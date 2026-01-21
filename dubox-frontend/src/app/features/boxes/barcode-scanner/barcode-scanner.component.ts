import { Component, EventEmitter, Input, Output, OnInit, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PanelService } from '../../../core/services/panel.service';
import { BrowserMultiFormatReader, NotFoundException } from '@zxing/library';

export interface ScanResult {
  barcode: string;
  scanType: 'SiteArrival' | 'Installation';
  success: boolean;
  panelName?: string;
  message: string;
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

  manualBarcode = '';
  scanning = false;
  error = '';
  successMessage = '';
  latitude: number | null = null;
  longitude: number | null = null;
  
  // Camera scanning
  isCameraMode = false;
  cameraActive = false;
  stream: MediaStream | null = null;
  barcodeDetector: any = null;
  zxingReader: BrowserMultiFormatReader | null = null;
  scanInterval: any = null;
  cameraSupported = false;
  useZXing = false; // Flag to use ZXing instead of BarcodeDetector

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
    // Check if BarcodeDetector API is available
    this.initializeBarcodeDetector();
  }

  ngOnDestroy(): void {
    this.stopCamera();
  }

  checkCameraSupport(): void {
    // Check if getUserMedia is supported
    this.cameraSupported = !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
  }

  async initializeBarcodeDetector(): Promise<void> {
    // Check if BarcodeDetector API is available (Chrome/Edge)
    if ('BarcodeDetector' in window) {
      try {
        this.barcodeDetector = new (window as any).BarcodeDetector({
          formats: ['qr_code', 'ean_13', 'code_128', 'ean_8', 'data_matrix']
        });
        this.useZXing = false;
        console.log('Using native BarcodeDetector API');
      } catch (e) {
        console.warn('BarcodeDetector initialization failed:', e);
        this.useZXing = true;
      }
    } else {
      // Fallback to ZXing for browsers without BarcodeDetector
      console.log('BarcodeDetector not available, using ZXing library');
      this.useZXing = true;
      this.zxingReader = new BrowserMultiFormatReader();
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

  async scanManualBarcode(): Promise<void> {
    if (!this.manualBarcode.trim()) {
      this.error = 'Please enter a barcode';
      return;
    }

    await this.processBarcode(this.manualBarcode.trim());
  }

  async processBarcode(barcode: string): Promise<void> {
    this.scanning = true;
    this.error = '';
    this.successMessage = '';

    const command = {
      barcode: barcode,
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
          barcode: barcode,
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
        this.error = err?.error?.message || err?.message || 'Failed to scan barcode. Panel not found.';
        
        const scanResult: ScanResult = {
          barcode: barcode,
          scanType: this.scanType,
          success: false,
          message: this.error
        };

        this.scanComplete.emit(scanResult);
      }
    });
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
        return 'Scan panel barcode to mark as arrived at site and approve';
      case 'Installation':
        return 'Scan panel barcode to mark as installed on site';
      default:
        return 'Scan panel barcode';
    }
  }

  clearInput(): void {
    this.manualBarcode = '';
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
      const constraints = [
        // Try 1: Back camera with ideal resolution
        {
          video: {
            facingMode: 'environment',
            width: { ideal: 1280 },
            height: { ideal: 720 }
          }
        },
        // Try 2: Back camera with default resolution
        {
          video: {
            facingMode: 'environment'
          }
        },
        // Try 3: Any camera with ideal resolution
        {
          video: {
            width: { ideal: 1280 },
            height: { ideal: 720 }
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
          
          // Clear any previous error about barcode detection
          if (this.error && (this.error.includes('Automatic barcode scanning') || this.error.includes('not supported'))) {
            this.error = '';
          }
          
          // Wait a moment for video to stabilize, then start scanning
          setTimeout(() => {
            console.log('Starting barcode scanning after video stabilization...');
            this.startScanning();
          }, 500);
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
    if (!this.barcodeDetector && !this.zxingReader) {
      // No barcode detection available
      console.warn('No barcode detection method available');
      this.error = 'Automatic barcode scanning is not available. Please use manual entry.';
      return;
    }

    // Clear any existing interval
    if (this.scanInterval) {
      clearInterval(this.scanInterval);
    }

    console.log('Starting barcode scanning...', {
      barcodeDetector: !!this.barcodeDetector,
      zxingReader: !!this.zxingReader,
      useZXing: this.useZXing
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

        let barcodeText: string | null = null;

        // Method 1: Use native BarcodeDetector API (Chrome/Edge)
        if (this.barcodeDetector && !this.useZXing) {
          try {
            // Use BarcodeDetector directly on video element (like user's example)
            const barcodes = await this.barcodeDetector.detect(video);
            
            if (barcodes && barcodes.length > 0) {
              const barcode = barcodes[0];
              if (barcode.rawValue) {
                barcodeText = barcode.rawValue;
                console.log('✅ Barcode detected (BarcodeDetector):', barcodeText);
                return barcodeText;
              }
            }
          } catch (err: any) {
            // Only log non-expected errors
            if (!(err instanceof Error && (err.name === 'NotFoundError' || err.name === 'NotReadableError'))) {
              console.warn('BarcodeDetector detection error:', err);
            }
          }
        }

        // Method 2: Use ZXing library (fallback for all browsers)
        if (!barcodeText && this.zxingReader && this.useZXing) {
          // Try decoding directly from video element (fastest)
          try {
            const result = await this.zxingReader.decode(video);
            if (result && result.getText()) {
              barcodeText = result.getText();
              console.log('✅ Barcode detected (ZXing from video):', barcodeText);
              return barcodeText;
            }
          } catch (videoErr: any) {
            // NotFoundException is expected when no barcode found - try canvas approach
            if (!(videoErr instanceof NotFoundException)) {
              console.warn('ZXing video decode error, trying canvas approach:', videoErr);
            }
          }
          
          // Fallback: Use canvas to image approach
          if (!barcodeText) {
            try {
              const canvas = this.canvasElement?.nativeElement;
              if (canvas && canvas.getContext) {
                const context = canvas.getContext('2d');
                if (context) {
                  // Draw video frame to canvas
                  canvas.width = video.videoWidth;
                  canvas.height = video.videoHeight;
                  context.drawImage(video, 0, 0, canvas.width, canvas.height);

                  // Convert canvas to image element for ZXing
                  const image = new Image();
                  image.crossOrigin = 'anonymous';
                  
                  // Use promise to handle image loading
                  const imageLoadPromise = new Promise<void>((resolve) => {
                    const timeout = setTimeout(() => resolve(), 150);
                    
                    image.onload = () => {
                      clearTimeout(timeout);
                      resolve();
                    };
                    
                    image.onerror = () => {
                      clearTimeout(timeout);
                      resolve();
                    };
                  });
                  
                  image.src = canvas.toDataURL('image/png');
                  await imageLoadPromise;
                  
                  // Only decode if image loaded successfully
                  if (image.complete && image.naturalWidth > 0) {
                    try {
                      const result = this.zxingReader!.decode(image);
                      if (result && result.getText()) {
                        barcodeText = result.getText();
                        console.log('✅ Barcode detected (ZXing from image):', barcodeText);
                        return barcodeText;
                      }
                    } catch (imageErr: any) {
                      // NotFoundException is normal when no barcode found - don't log
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
        }

        return barcodeText;
      } finally {
        // Always reset processing flag, even if an error occurs
        isProcessing = false;
      }
    };

    this.scanInterval = setInterval(async () => {
      if (!this.cameraActive) {
        return;
      }

      try {
        const barcodeText = await performScan();
        
        // Process detected barcode
        if (barcodeText) {
          console.log('🎯 Processing barcode:', barcodeText);
          // Clear interval first to stop scanning
          if (this.scanInterval) {
            clearInterval(this.scanInterval);
            this.scanInterval = null;
          }
          // Stop camera and process barcode
          this.stopCamera();
          this.manualBarcode = barcodeText;
          await this.processBarcode(barcodeText);
        }
      } catch (err) {
        console.error('Scanning error:', err);
      }
    }, 200); // Check every 200ms for faster detection
    
    console.log('✅ Barcode scanning interval started');
  }

  closeScanner(): void {
    this.stopCamera();
    this.close.emit();
  }

  /**
   * Check if barcode detection is available (either BarcodeDetector or ZXing)
   */
  isBarcodeDetectionAvailable(): boolean {
    return !!(this.barcodeDetector || this.zxingReader);
  }
}

