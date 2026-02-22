import { Directive, ElementRef, Input, OnChanges } from '@angular/core';

@Directive({
  selector: '[appIndeterminate]',
  standalone: true
})
export class IndeterminateDirective implements OnChanges {
  @Input() appIndeterminate: boolean = false;

  constructor(private el: ElementRef) {}

  ngOnChanges(): void {
    if (this.el.nativeElement) {
      this.el.nativeElement.indeterminate = this.appIndeterminate;
    }
  }
}
