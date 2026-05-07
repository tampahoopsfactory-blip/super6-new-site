export const siteConfig = {
  name: "Super6 Series LLC",
  tagline: "#1 Tournament Organization in Florida",
  description:
    "At Super6 Series LLC, our mission is to bridge the education gap for underserved youth and prepare them for lasting success beyond the playing field. We deliver programs that build a strong academic foundation through the power of sports.",
  url: "https://www.thesuper6.com",
  phone: "813-563-9767",
  email: "tk@thesuper6.com",
  address: "924 N Magnolia Avenue Suite 202-1151, Orlando, FL 32803",
  social: {
    instagram: "https://www.instagram.com/super6florida/",
    twitter: "https://twitter.com/TheSuper6Series",
    facebook: "https://www.facebook.com/thesuper6series/",
    youtube: "https://youtu.be/4Kv2O-sMPmY?list=PLkRC0fweoTKx6VTSY3AcbVbOMkev2RjGg",
  },
} as const;

/** US display numbers like 813-563-9767 → `sms:+18135639767` (prefer texting over voice). */
export function smsHrefFromDisplayPhone(display: string): string {
  const digits = display.replace(/\D/g, "");
  if (digits.length === 10) return `sms:+1${digits}`;
  if (digits.length === 11 && digits.startsWith("1")) return `sms:+${digits}`;
  return digits.length > 0 ? `sms:+${digits}` : "sms:";
}

export const siteSmsHref = smsHrefFromDisplayPhone(siteConfig.phone);

/* ─── Announcement bar messages ─── */
export const announcements = [
  {
    text: "2026 Season Registration Now Open",
    link: "/register",
    linkText: "Register",
  },
  {
    text: "NFHS-Certified Officials at Every Tournament",
    link: "/about",
    linkText: "Our Standard",
  },
  {
    text: "Boys 12th–3rd · Girls 12th–6th · Three-Game Guarantee",
    link: "/register",
    linkText: "Sign Up",
  },
] as const;


/* ─── Locations ─── */
export type Location = {
  slug: string;
  name: string;
  city: string;
  state: string;
  address?: string;
  phone?: string;
  mapUrl?: string;
  comingSoon: boolean;
  image: string;
  description: string;
};

export const locations: Location[] = [
  {
    slug: "orlando",
    name: "Super6 Orlando",
    city: "Orlando",
    state: "FL",
    address: "924 N Magnolia Avenue Suite 202-1151, Orlando, FL 32803",
    phone: "813-563-9767",
    mapUrl:
      "https://www.google.com/maps/place/924+N+Magnolia+Ave,+Orlando,+FL+32803",
    comingSoon: false,
    image: "/media/curated/06-super6-banner-bokeh.jpg",
    description:
      "Our flagship venue. Custom court branding, NFHS-certified officials, and a championship atmosphere across the calendar.",
  },
  {
    slug: "clearwater",
    name: "Super6 Clearwater",
    city: "Clearwater",
    state: "FL",
    comingSoon: false,
    image: "/media/uploads/clearwater-action.jpg",
    description:
      "Elite youth basketball on the Tampa Bay coast. Family-friendly weekends with top regional talent.",
  },
  {
    slug: "tampa",
    name: "Super6 Tampa",
    city: "Tampa",
    state: "FL",
    comingSoon: false,
    image: "/media/curated/03-drive-isolation.jpg",
    description:
      "Tampa brings fierce competition and deep talent pools. Our events attract teams from across the Gulf Coast region.",
  },
  {
    slug: "boca-raton",
    name: "Super6 Boca Raton",
    city: "Boca Raton",
    state: "FL",
    comingSoon: false,
    image: "/media/curated/09-trophy-raise.jpg",
    description:
      "South Florida's premier youth basketball destination. Palm Beach County competition with coast-to-coast draw.",
  },
  {
    slug: "atlanta",
    name: "Super6 Atlanta",
    city: "Atlanta",
    state: "GA",
    comingSoon: true,
    image: "/media/curated/22-scorers-table.jpg",
    description:
      "Expanding to the Southeast's basketball capital. Super6 brings its championship format to Georgia in 2026.",
  },
];

/* ─── Hero — full-width ─── */
export const heroData = {
  video: "/media/videos/hero-clip-3.mp4",
  poster: "/media/videos/hero-clip-3-poster.jpg",
  headline: "Elevate Your Game",
  subhead: "#1 Tournament Organization in Florida",
  cta: { label: "Register Now", href: "/register" },
  ctaSecondary: { label: "Our Mission", href: "/about" },
};



/* ─── Registration tiers ─── */
const sharedFeatures = [
  "3 guaranteed games per event",
  "Professional referees (NFHS certified)",
  "Digital scorebook",
  "Priority bracket seeding",
  "Guaranteed championship bracket entry",
  "Team profile on Super6 website",
  "Team profile on Super6 app",
  "Early access to schedule",
  "Championship atmosphere",
];

export const registrationTiers = [
  {
    id: "single-tournament",
    name: "Single Tournament",
    price: 99,
    priceLabel: "$99",
    period: "per team / per event",
    features: sharedFeatures,
    cta: "Register Now",
    popular: false,
  },
  {
    id: "season-pass",
    name: "Season Package",
    price: 899,
    priceLabel: "$899",
    period: "per team / 10 events",
    features: sharedFeatures,
    cta: "Get Season Package",
    popular: true,
  },
];

/* ─── Footer columns ─── */
export const footerColumns = {
  help: [
    { label: "Contact Us", href: "/contact" },
    { label: "Tournament Rules", href: "/rules" },
    { label: "Registration Help", href: "/register" },
    { label: "FAQs", href: "/faq" },
  ],
  offerings: [
    { label: "Tournaments", href: "/register" },
    { label: "Season Package", href: "/register" },
    { label: "College Corner", href: "/about" },
    { label: "College Pipeline", href: "/about" },
    { label: "Hiring Referees", href: "/contact" },
  ],
  company: [
    { label: "Our Mission", href: "/about" },
    { label: "Inside Super6", href: "/about" },
    { label: "12 Years Strong", href: "/about" },
    { label: "Testimonials", href: "/about" },
    { label: "Privacy Policy", href: "/about" },
  ],
};
