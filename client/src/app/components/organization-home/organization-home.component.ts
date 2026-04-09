    import { CardModule } from 'primeng/card';
    import { ButtonModule } from 'primeng/button';
    import { DataViewModule } from 'primeng/dataview';
    import { CommonModule } from '@angular/common';
    import {  inject } from '@angular/core';
    import { Component, signal, computed } from '@angular/core';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { OrgService } from '../../../services/org.service';
import { Package } from '../package/package';
import { RouterOutlet } from '@angular/router'; 
import { ActivatedRoute } from '@angular/router';
import { ShoppingCart } from '../shopping-cart/shopping-cart';
    @Component({
      standalone: true,
      imports: [CommonModule, CardModule, DragDropModule,ButtonModule, DataViewModule, ShoppingCart, RouterOutlet],
      selector: 'app-organization-home',
      templateUrl: './organization-home.component.html',
      styleUrl: './organization-home.component.css'
    })
    export class OrganizationHomeComponent {
  public orgService = inject(OrgService); 
  private route = inject(ActivatedRoute);
  ngOnInit() {
    this.route.params.subscribe(params => {
      const slug = params['orgSlug'];
      if (slug) {
        this.orgService.loadOrganization(slug);
      }
    });
  }
    }