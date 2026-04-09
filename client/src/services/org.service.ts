import { BACKSLASH } from '@angular/cdk/keycodes';
import { Injectable, computed, signal } from '@angular/core';


export interface Organization {
  id: number;
  slug: string;
  name: string;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
  accentContrast: string; 
 Url: string;
}

@Injectable({
  providedIn: 'root'
})
export class OrgService {
  // 2. ניהול המצב של הארגון הנוכחי
  private currentOrgSignal = signal<Organization | null>(null);
  currentOrg = computed(() => this.currentOrgSignal());

  
  private organizations: Organization[] = [
    {
  id: 1,
  slug: 'ezer-mizion',
  name: 'עזר מציון',
  primaryColor: 'black', 
  secondaryColor: '#F4F7FA',
  accentColor: '#E30613', 
  accentContrast: '#FFFFFF', 
  Url: 'ezer-mizion'
},
    {
      id: 2,
      slug: 'yad-sara',
      name: 'יד שרה',
      primaryColor: '#064E3B', 
      secondaryColor: '#F0FFF4',
      accentColor: '#B8860B', 
      accentContrast: '#FFFFFF', // טקסט לבן על ברונזה כהה
     Url: 'assets/logos/yad-sara.png'
    },
    {
  id: 3,
   slug: 'rabbis-relief',
  name: 'קופת צדקה', // דוגמה לשם ארגון
  primaryColor: '#721c24', // בורדו עמוק ומכובד
  secondaryColor: '#fdf2f2', // ורוד-בז' בהיר מאוד לרקע
  accentColor: '#c5a059', // זהב שמפניה עדין
  accentContrast: '#ffffff', // כיתוב לבן חד על הבורדו
  Url: 'assets/logos/charity-logo.png'
},
{
  id: 4,
  slug: 'medical-care',
  name: 'מרכז רפואי', 
  primaryColor: '#0F172A', // כחול נייבי עמוק מאוד
  secondaryColor: '#F1F5F9', // אפור-תכלת בהיר מאוד לרקע הטופס
  accentColor: '#94A3B8', // כסף-מתכתי עדין לפס העליון
  accentContrast: '#FFFFFF', // כיתוב לבן בוהק על הכחול הכהה
  Url: 'assets/logos/medical-center.png'
},
{
  id: 5,
  slug: 'united-hatzalah',
  name: 'איחוד הצלה', 
  primaryColor: '#F26522', // הכתום המזוהה של איחוד הצלה
  secondaryColor: '#F8F9FA', // רקע אפרפר-בהיר מאוד כדי לתת לכתום לבלוט
  accentColor: '#000000', // כחול כהה (Navy) לפס העליון או אלמנטים משלימים
  accentContrast: '#FFFFFF', // כיתוב לבן בוהק על הכתום
  Url: 'assets/logos/united-hatzalah.png'
},
{
  id: 6,
  slug: 'mda-digital',
  name: 'מגן דוד אדום', 
  primaryColor: '#D32F2F',    // אדום עוצמתי ומקצועי
  secondaryColor: '#FFFFFF',  // לבן נקי כצבע רקע מרכזי
  accentColor: '#1565C0',     // כחול רויאל עמוק (ליצירת ניגודיות רפואית קלאסית)
  accentContrast: '#F5F5F5',  // אפור בהיר מאוד לטקסט משני או רקע כפתור
  Url: 'assets/logos/mda-logo.png'
},
{
id: 7,
  slug: 'cyber-rescue',
  name: 'סייבר רסקיו',
  
  // צבעים
  primaryColor: '#4A148C',    // סגול עמוק (Deep Purple) - משדר בינה וטכנולוגיה
  secondaryColor: '#263238',  // אפור חלל (Space Grey) - רקע מקצועי ולא מעייף
  accentColor: '#00E676',     // ירוק מנטה זוהר (Neon Mint) - לאלמנטים של פעולה וחיים
  accentContrast: '#FFFFFF',  // לבן נקי לקריאות מקסימלית
  
  // נכסים
 Url: 'assets/logos/cyber-rescue-neon.png'
},
{
  id: 8,
  slug: 'maccabi-health',
  name: 'מכבי שירותי בריאות',
  primaryColor: '#0054A6',
  secondaryColor: '#FFFFFF',
  accentColor: '#00ADEF',
  accentContrast: '#FFFFFF',
  Url: 'assets/logos/maccabi.png'
},
 
{
  id: 10,
  slug: 'yedidim-official',
  name: 'ידידים - סיוע בדרכים',
  
  // צבעים מרכזיים
  primaryColor: '#003D5B',    // הכחול העמוק מהלוגו
  secondaryColor: '#FFB84D',  // הכתום-צהוב החם
  accentColor: '#E94E1B',     // האדום הבוהק
  
  accentContrast: '#FFFFFF',
  Url: 'assets/logos/yedidim_new.png'
}
  ];

  constructor() {}

  // 4. פונקציית הטעינה שמפעילה את השינוי בכל האתר
loadOrganization(slug: string) {
  const org = this.organizations.find(o => o.slug === slug);
  if (org) {
    this.currentOrgSignal.set(org);
    // הקריאה החסרה שמעדכנת את כל ה-CSS באתר!
    this.updateGlobalStyles(org); 
  }
}
// org.service.ts
getAssetsPath(): string {
  const org = this.currentOrg();
  // if (!org) return 'assets/placeholder-gifts/';
  
  // בניית הנתיב לפי המבנה שראינו בתיקיות שלך
  return `/${this.currentOrg()?.slug}/images/gifts/`;
}
private updateGlobalStyles(org: Organization) {
  const root = document.documentElement;
  
  // כאן אנחנו מחברים את המשתנים מה-CSS (styles.css) לערכים מה-JSON
  root.style.setProperty('--org-primary', org.primaryColor);
  root.style.setProperty('--primary-color', org.primaryColor);
  root.style.setProperty('--secondary-color', org.secondaryColor);
  root.style.setProperty('--accent-color', org.accentColor);
  root.style.setProperty('--accent-contrast', org.accentContrast);
  
  console.log(`Styles updated for: ${org.name}`); // לבדיקה ב-Console
}}