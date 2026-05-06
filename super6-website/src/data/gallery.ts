/* ─────────────────────────────────────────────────────────────────
   Galleries data — single source of truth.

   To swap images:
     1. Drop new photos into /public/media/<folder>/your-photo.jpg
     2. Update the `src` field below to /media/<folder>/your-photo.jpg
     3. Edit `title`, `event`, `year`, `category`, `alt`, `featured`,
        `aspect` to match. Page rebuilds automatically.

   To use Unsplash placeholders instead:
     1. Replace `src` with the Unsplash URL (e.g. https://images.unsplash.com/...)
     2. Add `{ protocol: "https", hostname: "images.unsplash.com" }` to
        `images.remotePatterns` in next.config.ts.

   `featured: true` makes a tile span 2 columns (spotlight).
   `aspect` controls the tile's aspect ratio — pick whatever frames best:
     "portrait" (3/4)  | "square" (1/1)  | "landscape" (4/3)
     "wide" (16/10)    | "tall" (4/5)    | "ultra" (16/9)
   ───────────────────────────────────────────────────────────────── */

export type GalleryCategory =
  | "tournament-action"
  | "behind-the-scenes"
  | "venues-facilities"
  | "athletes-champions"
  | "brand-moments";

export type GalleryAspect =
  | "portrait"
  | "square"
  | "landscape"
  | "wide"
  | "tall"
  | "ultra";

export type GalleryPhoto = {
  id: string;
  src: string;
  alt: string;
  title: string;        // e.g. "[Event Name - Year]"
  event: string;        // e.g. "Super6 Series LLC Showcase, Tampa"
  year: number;         // e.g. 2024
  category: GalleryCategory;
  featured?: boolean;   // span 2 cols
  aspect?: GalleryAspect;
};

export const CATEGORIES: { id: GalleryCategory | "all"; label: string }[] = [
  { id: "all",                label: "All" },
  { id: "tournament-action",  label: "Tournament Action" },
  { id: "behind-the-scenes",  label: "Behind the Scenes" },
  { id: "venues-facilities",  label: "Venues & Facilities" },
  { id: "athletes-champions", label: "Athletes & Champions" },
  { id: "brand-moments",      label: "Brand Moments" },
];

export const CATEGORY_META: Record<GalleryCategory, { label: string; eyebrow: string; blurb: string }> = {
  "tournament-action": {
    label: "Tournament Action",
    eyebrow: "On the Court",
    blurb: "Game-defining moments — drives, dunks, contests, and the split-seconds the camera caught right.",
  },
  "behind-the-scenes": {
    label: "Behind the Scenes",
    eyebrow: "How It Gets Built",
    blurb: "The crew, the calls, the clipboard. The work that happens before the first whistle.",
  },
  "venues-facilities": {
    label: "Venues & Facilities",
    eyebrow: "The Rooms We Take Over",
    blurb: "Multi-court setups, packed crowds, the scale of a Super6 Series LLC weekend.",
  },
  "athletes-champions": {
    label: "Athletes & Champions",
    eyebrow: "Trophy Lifts",
    blurb: "The moments families travel for. Champions, awards, and the celebrations earned.",
  },
  "brand-moments": {
    label: "Brand Moments",
    eyebrow: "Off the Court",
    blurb: "Sponsors, partners, banners, and the press cycle that's grown alongside the league.",
  },
};

/* ─── Photos. Order here = display order within each category. ─── */
export const PHOTOS: GalleryPhoto[] = [
  /* Tournament Action ─────────────────────────────── */
  {
    id: "ta-01", src: "/media/gallery/G1_01_Dunk_Action.jpg",
    alt: "Player rising for a dunk above the rim",
    title: "[Above the Rim — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "tournament-action", featured: true, aspect: "wide",
  },
  {
    id: "ta-02", src: "/media/gallery/G1_30_Fast_Break.jpg",
    alt: "Fast-break possession in transition",
    title: "[Fast Break — 2023]", event: "Spring Classic, Orlando", year: 2023,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-03", src: "/media/gallery/G1_27_Contested_Drive.jpg",
    alt: "Player driving against contested defense",
    title: "[Contested Drive — 2024]", event: "Summer Slam, Tampa", year: 2024,
    category: "tournament-action", aspect: "portrait",
  },
  {
    id: "ta-04", src: "/media/gallery/G1_03_One_On_One_Matchup.jpg",
    alt: "One-on-one matchup at the top of the key",
    title: "[Iso at the Elbow — 2023]", event: "Atlanta Showout", year: 2023,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-05", src: "/media/gallery/G1_17_Rebounding_Action.jpg",
    alt: "Rebound battle in the paint",
    title: "[Boards — 2022]", event: "Fall Tip-Off, Tampa", year: 2022,
    category: "tournament-action", aspect: "landscape",
  },
  {
    id: "ta-06", src: "/media/gallery/G2_33_Fast_Break_Rush.jpg",
    alt: "Open court at full speed",
    title: "[Open Floor — 2025]", event: "Spring Classic, Orlando", year: 2025,
    category: "tournament-action", aspect: "portrait",
  },
  {
    id: "ta-07", src: "/media/gallery/G1_32_Contest_Layup.jpg",
    alt: "Contested layup at the rim",
    title: "[At the Rim — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-08", src: "/media/gallery/G1_14_Free_Throw_Portrait.jpg",
    alt: "Free-throw routine, player at the line",
    title: "[The Line — 2023]", event: "Atlanta Showout", year: 2023,
    category: "tournament-action", aspect: "portrait",
  },
  {
    id: "ta-09", src: "/media/gallery/G2_21_Rebounding_Boxout.jpg",
    alt: "Boxout under the rim",
    title: "[Boxout — 2025]", event: "Summer Slam, Tampa", year: 2025,
    category: "tournament-action", featured: true, aspect: "wide",
  },
  {
    id: "ta-10", src: "/media/gallery/G2_30_Contested_Drive_Action.jpg",
    alt: "Defensive contest on a baseline drive",
    title: "[Baseline Contest — 2025]", event: "Fall Tip-Off, Tampa", year: 2025,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-11", src: "/media/curated/08-fast-break.jpg",
    alt: "Sprinting up the floor on a fast break",
    title: "[In Transition — 2022]", event: "Atlanta Showout", year: 2022,
    category: "tournament-action", aspect: "landscape",
  },
  {
    id: "ta-12", src: "/media/gallery/G2_18_Free_Throw_Action.jpg",
    alt: "Free-throw release",
    title: "[Release — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "tournament-action", aspect: "portrait",
  },

  /* Behind the Scenes ─────────────────────────────── */
  {
    id: "bts-01", src: "/media/uploads/bks-bag-check.jpg",
    alt: "Big Kelly's Security crew checking bags at the entrance",
    title: "[Security Check-in — 2025]", event: "Super6 Series LLC Showcase, Tampa", year: 2025,
    category: "behind-the-scenes", featured: true, aspect: "wide",
  },
  {
    id: "bts-02", src: "/media/gallery/G1_15_Coach_Clipboard.jpg",
    alt: "Coach with clipboard during timeout",
    title: "[The Clipboard — 2023]", event: "Atlanta Showout", year: 2023,
    category: "behind-the-scenes", aspect: "portrait",
  },
  {
    id: "bts-03", src: "/media/gallery/G2_10_Referee_Coach_Partnership.jpg",
    alt: "Referee and coach in conversation",
    title: "[Officials at Work — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "behind-the-scenes", aspect: "tall",
  },
  {
    id: "bts-04", src: "/media/uploads/refs-crew.jpg",
    alt: "Officiating crew before tip-off",
    title: "[The Crew — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "behind-the-scenes", aspect: "landscape",
  },
  {
    id: "bts-05", src: "/media/gallery/G1_04_Courtside_Coaching.jpg",
    alt: "Courtside coaching moment",
    title: "[Courtside — 2022]", event: "Spring Classic, Orlando", year: 2022,
    category: "behind-the-scenes", aspect: "portrait",
  },
  {
    id: "bts-06", src: "/media/gallery/G2_06_Coaching_Timeout.jpg",
    alt: "Timeout huddle around the clipboard",
    title: "[Timeout — 2025]", event: "Atlanta Showout", year: 2025,
    category: "behind-the-scenes", aspect: "tall",
  },

  /* Venues & Facilities ─────────────────────────────── */
  {
    id: "vf-01", src: "/media/gallery/G1_11_Packed_Crowd.jpg",
    alt: "Packed crowd in the stands",
    title: "[Sold Out — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "venues-facilities", featured: true, aspect: "wide",
  },
  {
    id: "vf-02", src: "/media/curated/24-panoramic-crowd.jpg",
    alt: "Panoramic view of the venue at full capacity",
    title: "[Full House — 2023]", event: "Atlanta Showout", year: 2023,
    category: "venues-facilities", featured: true, aspect: "ultra",
  },
  {
    id: "vf-03", src: "/media/gallery/G2_15_Packed_Crowd_Scene.jpg",
    alt: "Wide shot of packed bleachers",
    title: "[The Bleachers — 2025]", event: "Spring Classic, Orlando", year: 2025,
    category: "venues-facilities", aspect: "landscape",
  },
  {
    id: "vf-04", src: "/media/gallery/G1_05_Crowd_Spectator.jpg",
    alt: "Spectators along the sideline",
    title: "[Sideline View — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "venues-facilities", aspect: "tall",
  },

  /* Athletes & Champions ─────────────────────────────── */
  {
    id: "ac-01", src: "/media/curated/09-trophy-raise.jpg",
    alt: "Trophy raised after championship win",
    title: "[Trophy Lift — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "athletes-champions", featured: true, aspect: "wide",
  },
  {
    id: "ac-02", src: "/media/gallery/G1_02_Victory_Celebration.jpg",
    alt: "Team celebration on the court",
    title: "[The Celebration — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "athletes-champions", aspect: "portrait",
  },
  {
    id: "ac-04", src: "/media/gallery/G1_08_Team_Huddle_Celebration.jpg",
    alt: "Team huddle in celebration",
    title: "[Team Huddle — 2023]", event: "Atlanta Showout", year: 2023,
    category: "athletes-champions", aspect: "landscape",
  },
  {
    id: "ac-05", src: "/media/curated/13-team-king-crown.jpg",
    alt: "Champions with crowns and trophy",
    title: "[Crowned — 2022]", event: "Atlanta Showout", year: 2022,
    category: "athletes-champions", aspect: "portrait",
  },
  {
    id: "ac-06", src: "/media/curated/23-female-athlete.jpg",
    alt: "Athlete portrait",
    title: "[Player Portrait — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "athletes-champions", aspect: "tall",
  },
  {
    id: "ac-07", src: "/media/gallery/G1_20_Halftime_Celebration.jpg",
    alt: "Halftime moment of celebration",
    title: "[Halftime Lead — 2023]", event: "Spring Classic, Orlando", year: 2023,
    category: "athletes-champions", aspect: "landscape",
  },
  {
    id: "ac-08", src: "/media/gallery/G2_05_Three_Staff_Celebration.jpg",
    alt: "Staff celebrating with the team",
    title: "[Staff & Team — 2025]", event: "Fall Tip-Off, Tampa", year: 2025,
    category: "athletes-champions", aspect: "portrait",
  },

  /* Brand Moments ─────────────────────────────── */
  {
    id: "bm-01", src: "/media/curated/06-super6-banner-bokeh.jpg",
    alt: "Super6 Series LLC banner in soft focus on the court",
    title: "[The Banner — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "brand-moments", featured: true, aspect: "wide",
  },
  {
    id: "bm-02", src: "/media/uploads/celtics-super6.jpg",
    alt: "Super6 Series LLC alongside professional team partnership",
    title: "[Pro Partnership — 2025]", event: "Boston Celtics × Super6 Series LLC", year: 2025,
    category: "brand-moments", aspect: "landscape",
  },
  {
    id: "bm-03", src: "/media/gallery/G2_04_Solo_Coach_With_Banner.jpg",
    alt: "Coach in front of Super6 Series LLC brand wall",
    title: "[Brand Wall — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "brand-moments", aspect: "portrait",
  },
  {
    id: "bm-04", src: "/media/gallery/G2_11_Youth_Team_Promo.jpg",
    alt: "Youth team promotional shot",
    title: "[Team Promo — 2025]", event: "Atlanta Showout", year: 2025,
    category: "brand-moments", aspect: "tall",
  },
  {
    id: "bm-05", src: "/media/uploads/img-22-jersey.jpg",
    alt: "Custom Super6 Series LLC jersey on the rack",
    title: "[Custom Threads — 2024]", event: "Super6 Series LLC Showcase, Tampa", year: 2024,
    category: "brand-moments", aspect: "portrait",
  },
];

/* ─── Through the Years — one defining frame per year.
       Edit `src` + `caption` to swap. The first year is the
       founding year (2014). Add or remove rows freely. ─── */
export type TimelineFrame = {
  year: number;
  yearLabel: string;     // "Year 1", "Year 13" etc.
  src: string;
  caption: string;
  alt: string;
};

export const TIMELINE: TimelineFrame[] = [
  { year: 2014, yearLabel: "Year 1",  src: "/media/curated/01-flagship-coach-pointer.jpg",  caption: "[The first whistle. One court, one weekend, one bracket.]", alt: "Coach directing at first Super6 Series LLC event" },
  { year: 2015, yearLabel: "Year 2",  src: "/media/uploads/timeline-2015-thunder-celebration.png", caption: "[The standard set. Same brackets, sharper edges.]", alt: "Thunder team celebrating with medals at Super6 Series LLC" },
  { year: 2016, yearLabel: "Year 3",  src: "/media/gallery/G1_05_Crowd_Spectator.jpg",      caption: "[Sidelines start to swell.]",                              alt: "Crowd along the sideline" },
  { year: 2017, yearLabel: "Year 4",  src: "/media/uploads/timeline-2017-cavaliers-drive.png", caption: "[New teams. New cities scouting in.]", alt: "Firehouse Cavaliers player driving to the basket at Super6 Series LLC" },
  { year: 2018, yearLabel: "Year 5",  src: "/media/curated/16-packed-sideline.jpg",         caption: "[First sold-out weekend.]",                                alt: "Packed sideline at Super6 Series LLC" },
  { year: 2019, yearLabel: "Year 6",  src: "/media/curated/17-young-spectators.jpg",        caption: "[Expansion year. The schedule doubles.]",                  alt: "Young spectators watching" },
  { year: 2020, yearLabel: "Year 7",  src: "/media/gallery/G2_19_Coaching_Huddle_Scene.jpg",caption: "[Through the shutdown — and back.]",                       alt: "Coaching huddle" },
  { year: 2021, yearLabel: "Year 8",  src: "/media/gallery/G1_08_Team_Huddle_Celebration.jpg", caption: "[The return. Louder than before.]",                     alt: "Team huddle celebration" },
  { year: 2022, yearLabel: "Year 9",  src: "/media/curated/24-panoramic-crowd.jpg",         caption: "[Multi-court weekends, full venues.]",                     alt: "Panoramic packed venue" },
  { year: 2023, yearLabel: "Year 10", src: "/media/curated/12-coach-intensity.jpg",         caption: "[Decade in. Officials certified across the bracket.]",    alt: "Coach in intense moment" },
  { year: 2024, yearLabel: "Year 11", src: "/media/curated/09-trophy-raise.jpg",            caption: "[Trophies handed out. Stories made.]",                     alt: "Champion lifting trophy" },
  { year: 2025, yearLabel: "Year 12", src: "/media/uploads/celtics-super6.jpg",             caption: "[Pro partnerships. League pass debuts.]",                  alt: "Pro partnership moment" },
];

export const HERO_BACKGROUND = "/media/curated/24-panoramic-crowd.jpg";

/* ─── Per-section theme.
       "dark"  = ink background, cream type, orange accents (cinematic)
       "cream" = standard editorial, cream background, ink type
   Adjust freely to re-balance the visual rhythm of the "All" view. ─── */
export const SECTION_THEME: Record<GalleryCategory, "dark" | "cream"> = {
  "tournament-action":  "dark",
  "behind-the-scenes":  "dark",
  "venues-facilities":  "dark",
  "athletes-champions": "dark",
  "brand-moments":      "dark",
};

/* ─── Spotlight banners — full-bleed cinematic breaks between
       category sections. One big photo, dark scrim, eyebrow +
       serif quote. Edit `src`, `eyebrow`, `quote`, `meta` freely.
       Add or remove records to change how many banners appear
       (they slot in between sections in declared order). ─── */
export type Spotlight = {
  id: string;
  src: string;
  alt: string;
  eyebrow: string;
  quote: string;
  meta: string;
  /* Where to render relative to category sections, when the user is
     on "All". The banner appears AFTER the named category. */
  placeAfter: GalleryCategory;
};

export const SPOTLIGHTS: Spotlight[] = [
  {
    id: "sp-01",
    src: "/media/curated/24-panoramic-crowd.jpg",
    alt: "Panoramic packed venue at a Super6 Series LLC weekend",
    eyebrow: "12 Years In",
    quote:
      "What started as one weekend on one court is now a circuit. The kids who came to play are now coaching the kids who came to watch them.",
    meta: "Tampa · 2024",
    placeAfter: "tournament-action",
  },
  {
    id: "sp-02",
    src: "/media/uploads/bks-bag-check.jpg",
    alt: "Security crew working an entrance line",
    eyebrow: "The Standard",
    quote:
      "We don't run weekends. We run rooms. Every entrance covered, every official certified, every minute on the clock earned.",
    meta: "The Super6 Series LLC Way",
    placeAfter: "athletes-champions",
  },
];

/* ─── Featured Year Reel — a single year treated as a photo essay.
       One spotlight photo + three supporting frames + intro copy.
       Renders between two category sections. Edit `year`, `intro`,
       and the `featuredId` / `supportingIds` (which point to
       PHOTOS records by id). ─── */
export type FeaturedYear = {
  year: number;
  yearLabel: string;
  eyebrow: string;
  headline: string;
  intro: string;
  featuredId: string;       // points to a PHOTOS.id
  supportingIds: string[];  // 3 photo ids from PHOTOS
  placeAfter: GalleryCategory;
};

export const FEATURED_YEAR: FeaturedYear = {
  year: 2024,
  yearLabel: "Year 11",
  eyebrow: "The Featured Year",
  headline: "2024 — the year the room got bigger.",
  intro:
    "Sold-out weekends. New venues. The first nationally-televised Super6 Series LLC broadcast on HBCU League Pass. A look back at the year that proved the league had outgrown the gym.",
  featuredId: "ac-01",                                   // big spotlight (trophy raise)
  supportingIds: ["ta-01", "vf-01", "bm-01"],            // dunk, packed crowd, banner
  placeAfter: "behind-the-scenes",
};
