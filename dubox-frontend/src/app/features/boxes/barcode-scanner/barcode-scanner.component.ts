import { Component, EventEmitter, Input, Output, OnInit, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PanelService } from '../../../core/services/panel.service';
import { BrowserMultiFormatReader, NotFoundException } from '@zxing/library';

export interface ScanResult {
  qrCode: string;  // QR code value (previously barcode)
  scanType: 'SiteArrival' | 'Installation';
  success: boolean;
  panelName?: string;
  message: string;
  // Backward compatibility
  barcode?: string;  // @deprecated Use qrCode instead
}

@Component({
  selector: 'app-barcode-scanner',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './barcode-scanner.component.html',
  styleUrl: './barcode-scanner.component.scss'
})
export class BarcodeScannerComponent implements OnInit, OnDestroy, AfterViewChecked {
  @Input() projectId!: string;
  @Input() scanType: 'SiteArrival' | 'Installation' = 'SiteArrival';
  @Output() scanComplete = new EventEmitter<ScanResult>();
  @Output() close = new EventEmitter<void>();
  @ViewChild('videoElement') videoElement!: ElementRef<HTMLVideoElement>;
  @ViewChild('canvasElement') canvasElement!: ElementRef<HTMLCanvasElement>;

  manualQRCode = '';  // QR code input (previously manualBarcode)
  
  // Backward compatibility
  get manualBarcode(): string {
    return this.manualQRCode;
  }
  set manualBarcode(value: string) {
    this.manualQRCode = value;
  }
  scanning = false;
  error = '';
  successMessage = '';
  latitude: number | null = null;
  longitude: number | null = null;
  
  // Camera scanning
  isCameraMode = false;
  cameraActive = false;
  stream: MediaStream | null = null;
  zxingReader: BrowserMultiFormatReader | null = null;
  scanInterval: any = null;
  cameraSupported = false;

  private streamReady = false;

  constructor(
    private panelService: PanelService,
    private cdr: ChangeDetectorRef
  ) {
    this.getLocation();
    this.checkCameraSupport();
  }

  ngAfterViewChecked(): void {
    // Ensure video element gets the stream if it's ready but not attached
    if (this.streamReady && this.stream && this.videoElement?.nativeElement && !this.videoElement.nativeElement.srcObject) {
      this.attachStreamToVideo();
    }
  }

  ngOnInit(): void {
    // Initialize ZXing library for QR code scanning
    this.initializeQRCodeDetector();
  }

  ngOnDestroy(): void {
    this.stopCamera();
  }

  checkCameraSupport(): void {
    // Check if getUserMedia is supported
    this.cameraSupported = !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
  }

  async initializeQRCodeDetector(): Promise<void> {
    // Always use ZXing library for all browsers (Chrome, Edge, Firefox, Safari, etc.)
    // ZXing supports QR codes and multiple barcode formats
    // Configure to prioritize QR code scanning
    console.log('Initializing ZXing library for QR code scanning (all browsers)');
    try {
      // BrowserMultiFormatReader supports QR codes by default
      // QR codes are automatically detected along with other formats
      this.zxingReader = new BrowserMultiFormatReader();
      console.log('✅ ZXing library initialized successfully for QR code scanning');
    } catch (error: any) {
      console.error('❌ Failed to initialize ZXing library:', error);
      this.error = 'Failed to initialize QR code scanner. Please refresh the page.';
    }
  }

  getLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          this.latitude = position.coords.latitude;
          this.longitude = position.coords.longitude;
        },
        (error) => {
          console.warn('Geolocation not available:', error);
        }
      );
    }
  }

  async scanManualQRCode(): Promise<void> {
    if (!this.manualQRCode.trim()) {
      this.error = 'Please enter a QR code';
      return;
    }

    await this.processQRCode(this.manualQRCode.trim());
  }

  /**
   * Scan manual barcode (backward compatibility)
   * @deprecated Use scanManualQRCode instead
   */
  async scanManualBarcode(): Promise<void> {
    await this.scanManualQRCode();
  }

  async processQRCode(qrCode: string): Promise<void> {
    this.scanning = true;
    this.error = '';
    this.successMessage = '';

    // Backend API expects 'barcode' field, but contains QR code data
    const command = {
      barcode: qrCode,
      scanType: this.scanType,
      latitude: this.latitude ?? undefined,
      longitude: this.longitude ?? undefined
    };

    this.panelService.scanPanel(command).subscribe({
      next: (response: any) => {
        this.scanning = false;
        const result = response?.data || response;
        
        this.successMessage = `Panel scanned successfully! Status updated.`;
        
        const scanResult: ScanResult = {
          qrCode: qrCode,
          barcode: qrCode,  // Backward compatibility
          scanType: this.scanType,
          success: true,
          panelName: result?.panelName,
          message: this.successMessage
        };

        this.scanComplete.emit(scanResult);
        
        // Auto-close after 2 seconds
        setTimeout(() => {
          this.stopCamera();
          this.close.emit();
        }, 2000);
      },
      error: (err: any) => {
        this.scanning = false;
        this.error = err?.error?.message || err?.message || 'Failed to scan QR code. Panel not found.';
        
        const scanResult: ScanResult = {
          qrCode: qrCode,
          barcode: qrCode,  // Backward compatibility
          scanType: this.scanType,
          success: false,
          message: this.error
        };

        this.scanComplete.emit(scanResult);
      }
    });
  }

  /**
   * Process barcode (backward compatibility)
   * @deprecated Use processQRCode instead
   */
  async processBarcode(barcode: string): Promise<void> {
    await this.processQRCode(barcode);
  }

  getScanTypeLabel(): string {
    switch (this.scanType) {
      case 'SiteArrival':
        return 'Arrival at Site';
      case 'Installation':
        return 'Installation on Site';
      default:
        return 'Scan';
    }
  }

  getScanTypeDescription(): string {
    switch (this.scanType) {
      case 'SiteArrival':
        return 'Scan panel QR code to mark as arrived at site and approve';
      case 'Installation':
        return 'Scan panel QR code to mark as installed on site';
      default:
        return 'Scan panel QR code';
    }
  }

  clearInput(): void {
    this.manualQRCode = '';
    this.error = '';
    this.successMessage = '';
  }

  toggleCameraMode(): void {
    if (this.isCameraMode) {
      this.stopCamera();
    } else {
      this.startCamera();
    }
  }

  async startCamera(): Promise<void> {
    if (!this.cameraSupported) {
      this.error = 'Camera is not supported on this device';
      return;
    }

    try {
      this.isCameraMode = true;
      this.error = '';
      this.successMessage = '';
      this.streamReady = false;
      this.cdr.detectChanges(); // Force view update to show video element

      // Try multiple camera configurations as fallback
      // Use lower resolution for faster barcode detection (640x480 is sufficient for scanning)
      const constraints = [
        // Try 1: Back camera with optimized resolution for scanning (faster processing)
        {
          video: {
            facingMode: 'environment',
            width: { ideal: 640, max: 1280 },
            height: { ideal: 480, max: 720 }
          }
        },
        // Try 2: Back camera with default resolution
        {
          video: {
            facingMode: 'environment'
          }
        },
        // Try 3: Any camera with optimized resolution
        {
          video: {
            width: { ideal: 640, max: 1280 },
            height: { ideal: 480, max: 720 }
          }
        },
        // Try 4: Any camera with default settings
        {
          video: true
        }
      ];

      let lastError: any = null;
      
      for (let i = 0; i < constraints.length; i++) {
        try {
          console.log(`Trying camera constraint ${i + 1}/${constraints.length}:`, constraints[i]);
          this.stream = await navigator.mediaDevices.getUserMedia(constraints[i]);
          console.log('Camera stream obtained successfully with constraint:', constraints[i]);
          break; // Success, exit loop
        } catch (err: any) {
          console.warn(`Camera constraint ${i + 1} failed:`, err);
          lastError = err;
          
          // If this is the last attempt, we'll handle the error below
          if (i === constraints.length - 1) {
            throw err;
          }
          
          // Clean up any partial stream before trying next
          if (this.stream) {
            this.stream.getTracks().forEach(track => track.stop());
            this.stream = null;
          }
        }
      }

      if (!this.stream) {
        throw lastError || new Error('Could not access camera');
      }

      this.streamReady = true;
      this.cdr.detectChanges(); // Force view update

      // Wait a bit for Angular to render the video element
      await new Promise(resolve => setTimeout(resolve, 200));

      // Try to attach stream
      this.attachStreamToVideo();
    } catch (err: any) {
      this.isCameraMode = false;
      this.cameraActive = false;
      this.streamReady = false;
      
      // Clean up any stream that might have been created
      if (this.stream) {
        this.stream.getTracks().forEach(track => track.stop());
        this.stream = null;
      }
      
      if (err.name === 'NotAllowedError' || err.name === 'PermissionDeniedError') {
        this.error = 'Camera access denied. Please allow camera access in your browser settings and try again.';
      } else if (err.name === 'NotFoundError' || err.name === 'DevicesNotFoundError') {
        this.error = 'No camera found on this device.';
      } else if (err.name === 'NotReadableError' || err.name === 'TrackStartError') {
        this.error = 'Camera is already in use by another application. Please close other apps using the camera and try again.';
      } else if (err.name === 'OverconstrainedError' || err.name === 'ConstraintNotSatisfiedError') {
        this.error = 'Camera does not support the required settings. Please try using manual barcode entry.';
      } else if (err.message && err.message.includes('Could not start video source')) {
        this.error = 'Could not start camera. The camera may be in use by another application, or there may be a hardware issue. Please try closing other apps or restarting your device.';
      } else {
        this.error = 'Failed to access camera: ' + (err.message || err.name || 'Unknown error') + '. Please ensure no other application is using the camera.';
      }
      console.error('Camera error:', err);
    }
  }

  private attachStreamToVideo(): void {
    if (!this.stream || !this.videoElement?.nativeElement) {
      console.warn('Cannot attach stream: stream or video element not available');
      return;
    }

    const video = this.videoElement.nativeElement;
    
    // Check if stream is already attached
    if (video.srcObject === this.stream) {
      console.log('Stream already attached');
      return;
    }

    console.log('Attaching stream to video element');
    video.srcObject = this.stream;
    
    // Set video properties
    video.playsInline = true;
    video.muted = true; // Required for autoplay
    video.autoplay = true;
    
    // Set attributes for better compatibility
    video.setAttribute('playsinline', 'true');
    video.setAttribute('autoplay', 'true');
    video.setAttribute('muted', 'true');
    video.setAttribute('webkit-playsinline', 'true');
    
    // Wait for video to be ready
    const onLoadedMetadata = () => {
      console.log('Video metadata loaded, readyState:', video.readyState);
      console.log('Video dimensions:', video.videoWidth, 'x', video.videoHeight);
      
          video.play()
        .then(() => {
          console.log('Video playing successfully');
          this.cameraActive = true;
          this.cdr.detectChanges(); // Force view update
          
          // Clear any previous error about QR code detection
          if (this.error && (this.error.includes('Automatic QR code scanning') || this.error.includes('not supported'))) {
            this.error = '';
          }
          
          // Wait a moment for video to stabilize, then start scanning
          // Reduced delay for faster startup
          setTimeout(() => {
            console.log('Starting QR code scanning after video stabilization...');
            this.startScanning();
          }, 200);
          video.removeEventListener('loadedmetadata', onLoadedMetadata);
        })
        .catch((playError) => {
          console.error('Error playing video:', playError);
          this.error = 'Failed to start camera preview: ' + playError.message;
          this.stopCamera();
          video.removeEventListener('loadedmetadata', onLoadedMetadata);
        });
    };

    const onError = (error: any) => {
      console.error('Video error event:', error);
      this.error = 'Failed to display camera feed';
      this.stopCamera();
      video.removeEventListener('error', onError);
    };

    const onPlaying = () => {
      console.log('Video is now playing');
      video.removeEventListener('playing', onPlaying);
    };

    video.addEventListener('loadedmetadata', onLoadedMetadata);
    video.addEventListener('error', onError);
    video.addEventListener('playing', onPlaying);
    
    // Force load if already loaded
    if (video.readyState >= 2) {
      console.log('Video already loaded, calling onLoadedMetadata directly');
      onLoadedMetadata();
    }
  }

  stopCamera(): void {
    this.isCameraMode = false;
    this.cameraActive = false;
    this.streamReady = false;

    // Stop scanning interval
    if (this.scanInterval) {
      clearInterval(this.scanInterval);
      this.scanInterval = null;
    }

    // Stop ZXing reader if active
    if (this.zxingReader) {
      this.zxingReader.reset();
    }

    // Stop video stream
    if (this.stream) {
      this.stream.getTracks().forEach(track => {
        track.stop();
        console.log('Stopped track:', track.kind);
      });
      this.stream = null;
    }

    // Clear video element
    if (this.videoElement?.nativeElement) {
      const video = this.videoElement.nativeElement;
      video.srcObject = null;
      video.pause();
    }
  }

  startScanning(): void {
    if (!this.zxingReader) {
      // ZXing library not initialized - try to reinitialize
      console.warn('ZXing library not initialized, attempting to reinitialize...');
      this.initializeQRCodeDetector();
      
      // If still not available after reinitialization, show error
      if (!this.zxingReader) {
        this.error = 'Automatic QR code scanning is not available. Please use manual entry.';
        return;
      }
    }

    // Clear any existing interval
    if (this.scanInterval) {
      clearInterval(this.scanInterval);
    }

    console.log('Starting QR code scanning with ZXing library (all browsers)...', {
      zxingReader: !!this.zxingReader,
      browser: this.getBrowserInfo()
    });

    let isProcessing = false; // Prevent concurrent processing
    let scanCount = 0;

    const performScan = async (): Promise<string | null> => {
      if (!this.cameraActive || !this.videoElement?.nativeElement) {
        return null;
      }

      if (isProcessing) {
        return null; // Skip if already processing
      }

      const video = this.videoElement.nativeElement;
      
      // Ensure video has enough data and valid dimensions
      if (video.readyState !== video.HAVE_ENOUGH_DATA) {
        return null;
      }

      if (video.videoWidth === 0 || video.videoHeight === 0) {
        return null;
      }

      isProcessing = true;
      scanCount++;
      
      try {
        // Log every 10th scan for debugging
        if (scanCount % 10 === 0) {
          console.log(`🔍 Scanning attempt ${scanCount}...`, {
            videoReady: video.readyState === video.HAVE_ENOUGH_DATA,
            dimensions: `${video.videoWidth}x${video.videoHeight}`,
            isProcessing: false
          });
        }

        let qrCodeText: string | null = null;

        // Always use ZXing library for QR code detection in all browsers
        // ZXing works across Chrome, Edge, Firefox, Safari, and other modern browsers
        // ZXing automatically detects QR codes along with other formats
        if (!this.zxingReader) {
          console.warn('ZXing reader not available, attempting to reinitialize...');
          await this.initializeQRCodeDetector();
          if (!this.zxingReader) {
            return null;
          }
        }

        // Method 1: Try decoding directly from video element (fastest, works in all browsers)
        // ZXing automatically detects QR codes
        try {
          const result = await this.zxingReader.decode(video);
          if (result && result.getText()) {
            qrCodeText = result.getText();
            console.log('✅ QR code detected (ZXing from video - all browsers):', qrCodeText);
            return qrCodeText;
          }
        } catch (videoErr: any) {
          // NotFoundException is expected when no QR code found - try canvas approach
          if (!(videoErr instanceof NotFoundException)) {
            console.warn('ZXing video decode error, trying canvas approach:', videoErr);
          }
        }
        
        // Method 2: Fallback - Use canvas to image approach (works in all browsers)
        // This is a cross-browser compatible fallback for ZXing QR code detection
        if (!qrCodeText) {
          try {
            const canvas = this.canvasElement?.nativeElement;
            if (canvas && canvas.getContext) {
              const context = canvas.getContext('2d', { willReadFrequently: true });
              if (context) {
                // Use smaller canvas size for faster processing (scale down if video is large)
                const scaleFactor = video.videoWidth > 640 ? 0.5 : 1;
                canvas.width = Math.floor(video.videoWidth * scaleFactor);
                canvas.height = Math.floor(video.videoHeight * scaleFactor);
                
                // Draw video frame to canvas (scaled down for faster processing)
                context.drawImage(video, 0, 0, canvas.width, canvas.height);

                // Convert canvas to image element for ZXing (cross-browser compatible)
                const image = new Image();
                image.crossOrigin = 'anonymous';
                
                // Use promise to handle image loading with shorter timeout
                const imageLoadPromise = new Promise<void>((resolve) => {
                  const timeout = setTimeout(() => resolve(), 50); // Reduced from 150ms for faster processing
                  
                  image.onload = () => {
                    clearTimeout(timeout);
                    resolve();
                  };
                  
                  image.onerror = () => {
                    clearTimeout(timeout);
                    resolve();
                  };
                });
                
                // Use lower quality PNG for faster conversion (0.8 instead of default 0.92)
                image.src = canvas.toDataURL('image/png', 0.8);
                await imageLoadPromise;
                
                // Only decode if image loaded successfully
                if (image.complete && image.naturalWidth > 0) {
                  try {
                    const result = this.zxingReader!.decode(image);
                    if (result && result.getText()) {
                      qrCodeText = result.getText();
                      console.log('✅ QR code detected (ZXing from image - all browsers):', qrCodeText);
                      return qrCodeText;
                    }
                  } catch (imageErr: any) {
                    // NotFoundException is normal when no QR code found - don't log
                    if (!(imageErr instanceof NotFoundException)) {
                      console.warn('ZXing image decode error:', imageErr);
                    }
                  }
                }
              }
            }
          } catch (canvasErr: any) {
            // Only log non-NotFoundException errors
            if (!(canvasErr instanceof NotFoundException)) {
              console.warn('ZXing canvas detection error:', canvasErr);
            }
          }
        }

        return qrCodeText;
      } finally {
        // Always reset processing flag, even if an error occurs
        isProcessing = false;
      }
    };

    // Use optimized interval for faster detection
    // Reduced from 200ms to 100ms for faster barcode detection
    this.scanInterval = window.setInterval(async () => {
      if (!this.cameraActive) {
        return;
      }

      try {
        const qrCodeText = await performScan();
        
        // Process detected QR code
        if (qrCodeText) {
          console.log('🎯 Processing QR code:', qrCodeText);
          // Clear interval first to stop scanning
          if (this.scanInterval !== null) {
            clearInterval(this.scanInterval);
            this.scanInterval = null;
          }
          // Stop camera and process QR code
          this.stopCamera();
          this.manualQRCode = qrCodeText;
          await this.processQRCode(qrCodeText);
        }
      } catch (err) {
        console.error('Scanning error:', err);
      }
    }, 100); // Check every 100ms for faster detection (reduced from 200ms)
    
    console.log('✅ QR code scanning interval started');
  }

  closeScanner(): void {
    this.stopCamera();
    this.close.emit();
  }

  /**
   * Check if QR code detection is available (ZXing library)
   * ZXing works in all modern browsers: Chrome, Edge, Firefox, Safari, etc.
   * ZXing automatically detects QR codes along with other formats
   */
  isBarcodeDetectionAvailable(): boolean {
    return !!this.zxingReader;
  }

  /**
   * Get browser information for debugging
   */
  private getBrowserInfo(): string {
    const ua = navigator.userAgent;
    if (ua.includes('Chrome') && !ua.includes('Edg')) {
      return 'Chrome';
    } else if (ua.includes('Edg')) {
      return 'Edge';
    } else if (ua.includes('Firefox')) {
      return 'Firefox';
    } else if (ua.includes('Safari') && !ua.includes('Chrome')) {
      return 'Safari';
    } else {
      return 'Unknown';
    }
  }
}

