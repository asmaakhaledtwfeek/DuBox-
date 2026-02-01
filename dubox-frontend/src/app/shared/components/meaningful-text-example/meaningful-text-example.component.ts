import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { MeaningfulTextValidator } from '../../../core/validators/meaningful-text.validator';
import { MeaningfulTextDirective } from '../../directives/meaningful-text.directive';

/**
 * Example component demonstrating meaningful text validation usage
 * This component shows both reactive forms and template-driven forms approaches
 */
@Component({
  selector: 'app-meaningful-text-example',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MeaningfulTextDirective],
  template: `
    <div class="container mt-4">
      <h2>Meaningful Text Validation Examples</h2>
      
      <!-- Reactive Forms Example -->
      <div class="card mb-4">
        <div class="card-header">
          <h4>Example 1: Reactive Forms (Recommended)</h4>
        </div>
        <div class="card-body">
          <form [formGroup]="reactiveForm">
            <div class="mb-3">
              <label for="description" class="form-label">Issue Description *</label>
              <textarea 
                id="description"
                formControlName="description"
                class="form-control"
                rows="5"
                placeholder="Enter a detailed description (minimum 50 characters)..."
              ></textarea>
              
              <!-- Character Count -->
              <div class="form-text">
                <span [class.text-danger]="reactiveForm.get('description')?.value?.length < 50"
                      [class.text-success]="reactiveForm.get('description')?.value?.length >= 50">
                  {{ reactiveForm.get('description')?.value?.length || 0 }}/50 characters
                </span>
              </div>
              
              <!-- Validation Errors -->
              <div *ngIf="reactiveForm.get('description')?.invalid && reactiveForm.get('description')?.touched" 
                   class="alert alert-danger mt-2">
                <div *ngIf="reactiveForm.get('description')?.errors?.['required']">
                  Description is required.
                </div>
                <div *ngIf="reactiveForm.get('description')?.errors?.['meaningfulText']">
                  {{ reactiveForm.get('description')?.errors?.['meaningfulText']?.message }}
                </div>
              </div>
            </div>
            
            <button 
              type="button" 
              class="btn btn-primary"
              (click)="submitReactiveForm()"
              [disabled]="reactiveForm.invalid">
              Submit Reactive Form
            </button>
          </form>
        </div>
      </div>

      <!-- Template-Driven Forms Example -->
      <div class="card mb-4">
        <div class="card-header">
          <h4>Example 2: Template-Driven Forms</h4>
        </div>
        <div class="card-body">
          <form #templateForm="ngForm">
            <div class="mb-3">
              <label for="notes" class="form-label">Notes *</label>
              <textarea 
                id="notes"
                name="notes"
                [(ngModel)]="notes"
                meaningfulText
                required
                #notesField="ngModel"
                class="form-control"
                rows="5"
                placeholder="Enter meaningful notes (minimum 50 characters)..."
              ></textarea>
              
              <!-- Character Count -->
              <div class="form-text">
                <span [class.text-danger]="notes.length < 50"
                      [class.text-success]="notes.length >= 50">
                  {{ notes.length }}/50 characters
                </span>
              </div>
              
              <!-- Validation Errors -->
              <div *ngIf="notesField.invalid && notesField.touched" class="alert alert-danger mt-2">
                <div *ngIf="notesField.errors?.['required']">
                  Notes are required.
                </div>
                <div *ngIf="notesField.errors?.['meaningfulText']">
                  {{ notesField.errors?.['meaningfulText']?.message }}
                </div>
              </div>
            </div>
            
            <button 
              type="button" 
              class="btn btn-primary"
              (click)="submitTemplateForm()"
              [disabled]="templateForm.invalid">
              Submit Template Form
            </button>
          </form>
        </div>
      </div>

      <!-- Programmatic Validation Example -->
      <div class="card mb-4">
        <div class="card-header">
          <h4>Example 3: Programmatic Validation</h4>
        </div>
        <div class="card-body">
          <div class="mb-3">
            <label for="comment" class="form-label">Comment *</label>
            <textarea 
              id="comment"
              [(ngModel)]="comment"
              class="form-control"
              rows="5"
              placeholder="Enter a comment (minimum 50 characters)..."
            ></textarea>
            
            <!-- Character Count -->
            <div class="form-text">
              <span [class.text-danger]="comment.length < 50"
                    [class.text-success]="comment.length >= 50">
                {{ comment.length }}/50 characters
              </span>
            </div>
            
            <!-- Validation Error -->
            <div *ngIf="commentError" class="alert alert-danger mt-2">
              {{ commentError }}
            </div>
          </div>
          
          <button 
            type="button" 
            class="btn btn-primary"
            (click)="submitComment()">
            Submit Comment (Programmatic Validation)
          </button>
        </div>
      </div>

      <!-- Test Cases -->
      <div class="card mb-4">
        <div class="card-header">
          <h4>Test Cases</h4>
        </div>
        <div class="card-body">
          <div class="alert alert-info">
            <h5>Try these test cases:</h5>
            <ul>
              <li><strong>Valid:</strong> A detailed description with at least 50 characters that provides meaningful information about the issue, including context, impact, and any relevant technical details that would help someone understand the situation.</li>
              <li><strong>Too Short:</strong> Short text</li>
              <li><strong>Repeated Characters:</strong> aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</li>
              <li><strong>Keyboard Mashing:</strong> asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf</li>
              <li><strong>Low Variety:</strong> test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .char-count-info {
      font-size: 0.875rem;
      margin-top: 0.25rem;
    }
    
    .text-danger {
      color: #dc3545;
    }
    
    .text-success {
      color: #28a745;
    }
  `]
})
export class MeaningfulTextExampleComponent {
  // Reactive Form
  reactiveForm: FormGroup;

  // Template-Driven Form
  notes: string = '';

  // Programmatic Validation
  comment: string = '';
  commentError: string = '';

  constructor(private fb: FormBuilder) {
    // Initialize reactive form with meaningful text validator
    this.reactiveForm = this.fb.group({
      description: ['', [
        Validators.required,
        MeaningfulTextValidator.validate()
      ]]
    });
  }

  submitReactiveForm(): void {
    if (this.reactiveForm.valid) {
      console.log('Reactive form submitted:', this.reactiveForm.value);
      alert('Reactive form is valid and submitted successfully!');
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.reactiveForm.controls).forEach(key => {
        this.reactiveForm.get(key)?.markAsTouched();
      });
      alert('Please fix validation errors before submitting.');
    }
  }

  submitTemplateForm(): void {
    console.log('Template form submitted:', this.notes);
    alert('Template form is valid and submitted successfully!');
  }

  submitComment(): void {
    // Clear previous error
    this.commentError = '';

    // Validate programmatically
    const validationResult = MeaningfulTextValidator.isValid(this.comment);
    
    if (!validationResult.isValid) {
      this.commentError = validationResult.errorMessage || 'Invalid input';
      return;
    }

    console.log('Comment submitted:', this.comment);
    alert('Comment is valid and submitted successfully!');
    this.comment = '';
  }
}
