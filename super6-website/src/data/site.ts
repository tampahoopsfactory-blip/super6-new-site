export const siteConfig = {
  name: "Super 6",
  tagline: "#1 Tournament Organization in Florida",
  description:
    "At Super 6, our mission is to bridge the education gap for underserved youth and prepare them for lasting success beyond the playing field. We deliver programs that build a strong academic foundation through the power of sports.",
  url: "https://www.thesuper6.com",
  phone: "813-563-9767",
  email: "info@thesuper6.com",
  address: "924 N Magnolia Avenue Suite 202-1151, Orlando, FL 32803",
  social: {
    instagram: "https://www.instagram.com/super6florida/",
    twitter: "https://twitter.com/TheSuper6Series",
    facebook: "https://www.facebook.com/thesuper6series/",
    youtube: "https://youtu.be/4Kv2O-sMPmY?list=PLkRC0fweoTKx6VTSY3AcbVbOMkev2RjGg",
  },
} as const;

/* ─── Announcement bar messages ─── */
export const announcements = [
  {
    text: "Orlando $99 Tournament — Feb 28 – Mar 1, 2026",
    link: "/register",
    linkText: "Register Now",
  },
  {
    text: "New Location: Atlanta Coming Soon",
    link: "/locations/atlanta",
    linkText: "Learn More",
  },
  {
    text: "NFHS-Certified Referees at Every Tournament",
    link: "/about",
    linkText: "Learn More",
  },
  {
    text: "Boys 12th–3rd & Girls 12th–6th — 3 Game Guarantee",
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
    name: "Super 6 Orlando",
    city: "Orlando",
    state: "FL",
    address: "924 N Magnolia Avenue Suite 202-1151, Orlando, FL 32803",
    phone: "813-563-9767",
    mapUrl:
      "https://www.google.com/maps/place/924+N+Magnolia+Ave,+Orlando,+FL+32803",
    comingSoon: false,
    image: "/media/locations/court-branding.jpg",
    description:
      "Our flagship Orlando venue features professional courts, NFHS-certified referees, and a championship atmosphere for youth basketball tournaments year-round.",
  },
  {
    slug: "clearwater",
    name: "Super 6 Clearwater",
    city: "Clearwater",
    state: "FL",
    comingSoon: false,
    image: "/media/lifestyle/tournament-vibe.jpg",
    description:
      "Bringing elite youth basketball to the Tampa Bay coast. Clearwater tournaments feature top regional talent and a family-friendly competition environment.",
  },
  {
    slug: "tampa",
    name: "Super 6 Tampa",
    city: "Tampa",
    state: "FL",
    comingSoon: false,
    image: "/media/lifestyle/basketball-action.jpg",
    description:
      "The Tampa market brings fierce competition and deep talent pools. Our Tampa events attract teams from across the Gulf Coast region.",
  },
  {
    slug: "west-palm",
    name: "Super 6 West Palm",
    city: "West Palm Beach",
    state: "FL",
    comingSoon: false,
    image: "/media/lifestyle/game-intensity.jpg",
    description:
      "South Florida's premier youth basketball destination. West Palm Beach tournaments showcase the best talent from Miami-Dade to the Treasure Coast.",
  },
  {
    slug: "atlanta",
    name: "Super 6 Atlanta",
    city: "Atlanta",
    state: "GA",
    comingSoon: true,
    image: "/media/lifestyle/coach-strategy.jpg",
    description:
      "Expanding to the Southeast's basketball capital. Atlanta is coming soon — bringing Super 6's championship tournament format to Georgia's youth basketball scene.",
  },
];

/* ─── Hero — full-width ─── */
export const heroData = {
  video: "/media/videos/hero-clip-3.mp4",
  poster: "/media/videos/hero-clip-3-poster.jpg",
  headline: "Elevate Your Game",
  subhead: "#1 Tournament Organization in Florida",
  cta: { label: "Register Now", href: "/register" },
  ctaSecondary: { label: "Explore Tournaments", href: "/locations" },
};



/* ─── Registration tiers ─── */
export const registrationTiers = [
  {
    id: "single-tournament",
    name: "Single Tournament",
    price: 99,
    priceLabel: "$99",
    period: "per team / per event",
    features: [
      "3 guaranteed games minimum",
      "Professional referees (NFHS certified)",
      "Digital scorebook",
      "Championship atmosphere",
    ],
    cta: "Register Now",
    popular: false,
  },
  {
    id: "season-pass",
    name: "Season Pass",
    price: 350,
    priceLabel: "$350",
    period: "per team / full season",
    features: [
      "All regular-season tournaments included",
      "Priority bracket seeding",
      "Guaranteed championship bracket entry",
      "Team profile on Super 6 website",
      "Team profile on Super 6 app",
      "Early access to schedule",
    ],
    cta: "Get Season Pass",
    popular: true,
  },
  {
    id: "club-package",
    name: "Club Package",
    price: 1200,
    priceLabel: "$1,200",
    period: "up to 4 teams / full season",
    features: [
      "Everything in Season Pass for up to 4 teams",
      "Dedicated club coordinator",
      "Club branding on court signage",
      "Priority court scheduling",
      "Annual club showcase event",
      "Volume discount on additional teams",
    ],
    cta: "Contact Us",
    popular: false,
  },
];

/* ─── Footer columns ─── */
export const footerColumns = {
  help: [
    { label: "Contact Us", href: "/contact" },
    { label: "FAQs", href: "/about" },
    { label: "Tournament Rules", href: "/about" },
    { label: "Registration Help", href: "/register" },
    { label: "Tournament Info", href: "/about" },
    { label: "Find a Location", href: "/locations" },
  ],
  offerings: [
    { label: "Tournaments", href: "/register" },
    { label: "Season Pass", href: "/register" },
    { label: "Club Packages", href: "/register" },
    { label: "College Corner", href: "/about" },
    { label: "College Pipeline", href: "/about" },
    { label: "Hiring Referees", href: "/contact" },
  ],
  company: [
    { label: "Our Mission", href: "/about" },
    { label: "Inside Super 6", href: "/about" },
    { label: "12 Years Strong", href: "/about" },
    { label: "Testimonials", href: "/about" },
    { label: "Locations", href: "/locations" },
    { label: "Privacy Policy", href: "/about" },
  ],
};
