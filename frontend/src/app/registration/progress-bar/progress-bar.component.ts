import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrls: ['./SCSS/style.scss'],
})
export class ProgressBarComponent implements OnInit {
  @Input() step2: boolean = false;
  @Input() step3: boolean = false;
  constructor() {}
  step2Class() {
    return this.step2
      ? 'progress-numbers-circle complete'
      : 'progress-numbers-circle incomplete';
  }
  step3Class() {
    return this.step3
      ? 'progress-numbers-circle complete'
      : 'progress-numbers-circle incomplete';
  }
  ngOnInit(): void {}
}
