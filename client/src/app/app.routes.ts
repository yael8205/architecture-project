import { Routes } from '@angular/router';


import { OrganizationHomeComponent } from './components/organization-home/organization-home.component';
import { Package } from './components/package/package';
import { Gifts } from './components/gifts/gifts'; 
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { Video } from './components/video/video';
import { ManagerGift } from './components/manager-gift/manager-gift';

import { Header } from './components/header/header';
import { ManagerCategory } from './components/manager-category/manager-category';
import { ManagerPackage } from './components/manager-package/manager-package';
import { ManagerDonor } from './components/manager-donor/manager-donor';
import { LotteryComponent } from './components/lottery/lottery';
import { orders } from './components/orders/orders';
import { Checkout } from './components/check-out/check-out';
import { Manager } from './components/manager/manager';
import { ReportWinner } from './components/report-winner/report-winner';
import { RevenueReport } from './components/revenue-report/revenue-report';
export const routes: Routes = [
{ path: 'join/:orgSlug', component: OrganizationHomeComponent,
    children: [
      { path: 'packages', component: Package, }, // ילד 1: חבילות
      { path: 'gifts', component: Gifts },       // ילד 2: מתנות
      { path: '', redirectTo: 'packages', pathMatch: 'full' }
    ]
 },
{ path: '', redirectTo: 'join/united-hatzalah', pathMatch: 'full' },
{ path: 'join/:orgSlug/login', component: Login },
{ path: 'join/:orgSlug/register', component: Register },
{ path: 'join/:orgSlug/manager-gift', component: ManagerGift },
{ path: 'join/:orgSlug/manager-category', component: ManagerCategory },
{ path: 'join/:orgSlug/manager-package', component: ManagerPackage },
{ path: 'join/:orgSlug/manager-donor', component: ManagerDonor },
{ path: 'join/:orgSlug/manager-lottery', component: LotteryComponent },
{ path: 'join/:orgSlug/orders', component: orders },
{ path: 'join/:orgSlug/checkout', component: Checkout },
{ path: 'join/:orgSlug/manager', component: Manager },
{ path: 'join/:orgSlug/report-winner', component: ReportWinner },
{ path: 'join/:orgSlug/revenue-report', component: RevenueReport }


];
