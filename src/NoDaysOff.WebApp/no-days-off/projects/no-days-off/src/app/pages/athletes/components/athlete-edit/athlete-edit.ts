import { Component, input, output, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Athlete } from '../../../../models/athlete';

@Component({
  selector: 'ndo-athlete-edit',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './athlete-edit.html',
  styleUrl: './athlete-edit.scss'
})
export class AthleteEdit implements OnInit {
  athlete = input<Athlete | null>(null);

  saved = output<Athlete>();
  cancelled = output<void>();

  form!: FormGroup;
  isLoading = signal(false);

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    const athlete = this.athlete();
    this.form = this.fb.group({
      name: [athlete?.name ?? '', Validators.required],
      email: [athlete?.email ?? '', [Validators.required, Validators.email]],
      imageUrl: [athlete?.imageUrl ?? '']
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading.set(true);
      const athlete: Athlete = {
        athleteId: this.athlete()?.athleteId ?? 0,
        name: this.form.value.name,
        email: this.form.value.email,
        imageUrl: this.form.value.imageUrl,
        createdAt: this.athlete()?.createdAt ?? new Date()
      };
      this.saved.emit(athlete);
      this.isLoading.set(false);
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
