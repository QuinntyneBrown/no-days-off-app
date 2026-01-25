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
      username: [athlete?.username ?? '', Validators.required],
      imageUrl: [athlete?.imageUrl ?? '']
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading.set(true);
      const existingAthlete = this.athlete();
      const athlete: Athlete = {
        athleteId: existingAthlete?.athleteId ?? 0,
        name: this.form.value.name,
        username: this.form.value.username,
        imageUrl: this.form.value.imageUrl,
        createdOn: existingAthlete?.createdOn ?? new Date(),
        createdBy: existingAthlete?.createdBy ?? 'system'
      };
      this.saved.emit(athlete);
      this.isLoading.set(false);
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
