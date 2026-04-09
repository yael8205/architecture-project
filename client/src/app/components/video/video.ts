import { Component, effect, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { inject } from '@angular/core';
import { OrgService } from '../../../services/org.service';
import { JsonPipe } from '@angular/common';
@Component({
  selector: 'app-video',
  imports: [],
  templateUrl: './video.html',
  styleUrl: './video.css',
})
export class Video {
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
