import { Component, inject, computed, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, Router, NavigationEnd } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { OrgService } from '../../../services/org.service';
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
    const token = localStorage.getItem('token');
    this.isLoggedIn.set(!!token);
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const role = payload['role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        this.isAdmin.set(role === 'Admin' || role === 'Manager');
      } catch { this.isAdmin.set(false); }
    } else {
      this.isAdmin.set(false);
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate([`/join/${this.slug()}`]).then(() => {
      window.location.reload(); 
    });
  }
}