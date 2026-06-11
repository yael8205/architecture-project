import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; 
import { OrgService } from '../../../services/org.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password'; 
import { CardModule } from 'primeng/card'; 
import { FloatLabelModule } from 'primeng/floatlabel'; 
import { DividerModule } from 'primeng/divider';
import { ActivatedRoute } from '@angular/router'; 
import { LoginRequestDto } from '../../../models/auth.model';
import { AuthService } from '../../../services/auth.service';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
 imports: [
    FormsModule, 
    ButtonModule, 
    DividerModule, 
    InputTextModule, 
    PasswordModule, 
    CardModule, 
    FloatLabelModule,
    RouterModule,
  ],
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
})

export class Login {
  isLoading: boolean = false;
user: LoginRequestDto = {
    email: '',  
    password: '',  
  };
   private authService=inject(AuthService)
   public orgService = inject(OrgService);
  private route = inject(ActivatedRoute);
private router = inject(Router);
  ngOnInit() {
    this.route.params.subscribe(params => {
      const slug = params['orgSlug'];
      if (slug) {
        this.orgService.loadOrganization(slug);
      }
    });
  }
  onLogin() {
    if (!this.user.email || !this.user.password ) {
      alert('נא למלא את כל שדות החובה');
      return;
    }
  this.isLoading = true;
    this.authService.login(this.user).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response?.user) {
          localStorage.setItem('user', JSON.stringify(response.user));
          localStorage.removeItem('token');
          this.router.navigate(['/join', this.orgService.currentOrg()?.slug, 'organizations']);
          window.location.href = `/join/${this.orgService.currentOrg()?.slug}`;
        } else {
          console.error('התקבלה תשובה מהשרת ללא נתוני משתמש', response);
          alert('חלה שגיאה בקבלת נתוני הגישה מהשרת.');
        }
      },
     error: (err) => {
        this.isLoading = false;
        console.error('שגיאה בהתחברות:', err);
      }
    });
    
}



}