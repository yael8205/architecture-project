import { Component, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrgService } from '../../../services/org.service';
@Component({
  selector: 'app-manager',
  imports: [CommonModule, RouterLink],
  templateUrl: './manager.html',
  styleUrl: './manager.css',
})



export class Manager {
  private orgService = inject(OrgService);
  
  currentOrg = computed(() => this.orgService.currentOrg());
  slug = computed(() => this.currentOrg()?.slug);

  menuItems = computed(() => [
    { 
      title: 'ניהול תורמים', 
      icon: 'pi pi-users', 
      description: 'צפייה בפרטי תורמים, היסטוריית תרומות וסטטיסטיקות.',
      link: `/join/${this.slug()}/manager-donor` 
    },
    { 
      title: 'ניהול חבילות', 
      icon: 'pi pi-box', 
      description: 'הוספה, עריכה והסרה של חבילות תרומה למכירה.',
      link: `/join/${this.slug()}/manager-package` 
    },
    { 
      title: 'ניהול קטגוריות', 
      icon: 'pi pi-tags', 
      description: 'ארגון המתנות והחבילות לפי קטגוריות מותאמות.',
      link: `/join/${this.slug()}/manager-category` 
    },
    { 
      title: 'ניהול מתנות', 
      icon: 'pi pi-gift', 
      description: 'ניהול מלאי המתנות שניתן לזכות בהן בהגרלה.',
      link: `/join/${this.slug()}/manager-gift` 
    },
    { 
      title: 'הגרלה', 
      icon: 'pi pi-ticket', 
      description: 'ביצוע הגרלות, צפייה בזוכים וניהול כרטיסים.',
      link: `/join/${this.slug()}/manager-lottery` 
    },
     { 
      title: 'רשימת הזוכים', 
      icon: 'pi pi-ticket', 
      description: 'צפייה ברשימת הזוכים בהגרלות.',
      link: `/join/${this.slug()}/report-winner` 
    }, { 
      title: 'דוח הכנסות', 
      icon: 'pi pi-chart-line', 
      description: 'צפייה בדוח הכנסות של הארגון.',
      link: `/join/${this.slug()}/revenue-report` 
    }
  ]);
}