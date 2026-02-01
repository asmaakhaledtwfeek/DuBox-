import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';
import { MeaningfulTextValidator } from '../../core/validators/meaningful-text.validator';

/**
 * Directive for template-driven forms to validate meaningful text
 * Usage: <textarea meaningfulText [(ngModel)]="description"></textarea>
 */
@Directive({
  selector: '[meaningfulText]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: MeaningfulTextDirective,
      multi: true
    }
  ],
  standalone: true
})
export class MeaningfulTextDirective implements Validator {
  @Input() meaningfulText: boolean | '' = true;

  validate(control: AbstractControl): ValidationErrors | null {
    // If directive is disabled, skip validation
    if (this.meaningfulText === false) {
      return null;
    }

    return MeaningfulTextValidator.validate()(control);
  }
}
