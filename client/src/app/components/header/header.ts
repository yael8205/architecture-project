import { Component, inject, computed, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, Router, NavigationEnd } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { AuthService } from '../../../services/auth.service';
import { OrgService } from '../../../services/org.service';
import { UserRole } from '../../../models/auth.model';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, ButtonModule, TooltipModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header implements OnInit {
  private orgService = inject(OrgService);
  private authService = inject(AuthService);
  private router = inject(Router);

  isAdmin = signal(false);
  isLoggedIn = signal(false);

  currentOrg = computed(() => this.orgService.currentOrg());
  slug = computed(() => this.currentOrg()?.slug);
  logoPath = computed(() => `/${this.slug()}/images/logo.jpg`);

  loginLink = computed(() => `/join/${this.slug()}/login`);
  registerLink = computed(() => `/join/${this.slug()}/register`);
  managerLink = computed(() => `/join/${this.slug()}/manager`);

  navItems = computed(() => [
    { label: 'חבילות', icon: 'pi pi-shopping-bag', link: `/join/${this.slug()}/packages` },
    { label: 'מתנות', icon: 'pi pi-gift', link: `/join/${this.slug()}/gifts` },
    { label: 'הזמנות שלי', icon: 'pi pi-history', link: `/join/${this.slug()}/orders` }
  ]);

  ngOnInit() {
    this.checkAuth();
    this.router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => {
      this.checkAuth();
      const parts = this.router.url.split('/');
      const slug = parts[2];
      if (slug) this.orgService.loadOrganization(slug);
    });
  }

  private checkAuth() {
    const userJson = localStorage.getItem('user');
    this.isLoggedIn.set(!!userJson);
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        this.isAdmin.set(user.role === UserRole.Manager || user.role === 'Manager' || user.role === 'Admin');
      } catch {
        this.isAdmin.set(false);
      }
    } else {
      this.isAdmin.set(false);
    }
  }

  logout() {
    this.authService.logout().subscribe({
      complete: () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        this.router.navigate([`/join/${this.slug()}`]).then(() => {
          window.location.reload();
        });
      },
      error: () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        this.router.navigate([`/join/${this.slug()}`]).then(() => {
          window.location.reload();
        });
      }
    });
  }
}