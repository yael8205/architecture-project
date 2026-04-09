import { Component, inject, OnInit } from '@angular/core'; 
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { StepperModule } from 'primeng/stepper';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router'; 
import { UserCreateDto } from '../../../models/auth.model';
import { AuthService } from '../../../services/auth.service';
import { OrgService } from '../../../services/org.service'; 
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import { RouterModule } from '@angular/router';
import { switchMap } from 'rxjs';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { Router } from '@angular/router';
import { Route } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule, 
    ButtonModule, 
    PasswordModule, 
    StepperModule, 
    ToggleButtonModule, 
    InputTextModule, 
    CommonModule, 
    CardModule, 
    FloatLabelModule,
    RouterModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register implements OnInit { 
  activeStep: number = 1;
  isLoading: boolean = false;
  user: UserCreateDto = {
    name: '',
    email: '',
    password: '',
    phone: '',
    address: ''
  };

  private authService = inject(AuthService);
  public orgService = inject(OrgService); 
  private route = inject(ActivatedRoute);
  private cartService = inject(ShoppingCartService);
private router = inject(Router);
  ngOnInit() {
    this.route.params.subscribe(params => {
      const slug = params['orgSlug'];
      if (slug) {
        this.orgService.loadOrganization(slug);
      }
    });
  }

  onSubmit() {
    if (!this.user.name || !this.user.email || !this.user.password || !this.user.phone || !this.user.address) {
      alert('נא למלא את כל שדות החובה');
      return;
    }
  this.isLoading = true;
    this.authService.register(this.user).pipe(
    switchMap(() => this.authService.login({ email: this.user.email, password: this.user.password })),

    switchMap((loginResponse) => {
  if (loginResponse.token) {
        localStorage.setItem('token', loginResponse.token);
      }

      const newUserId = loginResponse.id || loginResponse.user?.id;
      
      return this.cartService.createCart({ participantId: newUserId }); 
    })).subscribe({
      next: (response) => {
        this.isLoading = false;
                    this.router.navigate(['/join', this.orgService.currentOrg()?.slug, 'login']); 

      },
      error: (err) => {
        this.isLoading = false;

        console.error('פרטי שגיאה:', err);
      }
    });
  }
}