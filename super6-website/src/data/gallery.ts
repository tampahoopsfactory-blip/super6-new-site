/* ─────────────────────────────────────────────────────────────────
   Galleries data — single source of truth.

   RULE: every photo path on this page must be UNIQUE — no photo
   used in PHOTOS may appear in TIMELINE, SPOTLIGHTS, or
   HERO_BACKGROUND, and no photo here may be referenced anywhere
   else on the site.
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
  title: string;
  event: string;
  year: number;
  category: GalleryCategory;
  featured?: boolean;
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
    blurb: "Multi-court setups, packed crowds, the scale of a Super6 weekend.",
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
    title: "[Above the Rim — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "tournament-action", featured: true, aspect: "wide",
  },
  {
    id: "ta-02", src: "/media/gallery/V_07_Point_Guard_Drive.jpg",
    alt: "Young point guard pushing the ball up the floor",
    title: "[Point Guard — 2023]", event: "Spring Classic, Orlando", year: 2023,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-03", src: "/media/gallery/G1_27_Contested_Drive.jpg",
    alt: "Player driving against contested defense",
    title: "[Contested Drive — 2024]", event: "Summer Slam, Tampa", year: 2024,
    category: "tournament-action", aspect: "portrait",
  },
  {
    id: "ta-04", src: "/media/gallery/V_01_Defensive_Matchup.jpg",
    alt: "Defender squared up on a ball-handler",
    title: "[Iso at the Top — 2023]", event: "Atlanta Showout", year: 2023,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-05", src: "/media/gallery/G1_25_Ready_Stance.jpg",
    alt: "Defender locked in a ready stance",
    title: "[Ready Stance — 2022]", event: "Fall Tip-Off, Tampa", year: 2022,
    category: "tournament-action", aspect: "landscape",
  },
  {
    id: "ta-06", src: "/media/gallery/G1_26_Ball_Handling.jpg",
    alt: "Choctaw Kings player handling the ball at the top of the key",
    title: "[Ball Handling — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "tournament-action", aspect: "portrait",
  },
  {
    id: "ta-07", src: "/media/gallery/V_22_Celtics_Drive.jpg",
    alt: "Celtics jersey point guard driving past defender",
    title: "[Celtics Drive — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
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
    id: "ta-10", src: "/media/gallery/V_18_Game_Action.jpg",
    alt: "Youth game action mid-possession",
    title: "[Mid-Possession — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "tournament-action", aspect: "tall",
  },
  {
    id: "ta-11", src: "/media/gallery/G1_30_Fast_Break.jpg",
    alt: "Sprinting up the floor on a fast break",
    title: "[In Transition — 2022]", event: "Atlanta Showout", year: 2022,
    category: "tournament-action", aspect: "landscape",
  },
  {
    id: "ta-12", src: "/media/gallery/G1_06_Youth_Action_SHOWOUT.jpg",
    alt: "Youth player attacking at the showout",
    title: "[Showout — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "tournament-action", aspect: "portrait",
  },

  /* Behind the Scenes ─────────────────────────────── */
  {
    id: "bts-01", src: "/media/gallery/V_24_Coach_Mentorship_Closeup.jpg",
    alt: "Coach making a point to a Legacy player on the sideline",
    title: "[The Lesson — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "behind-the-scenes", featured: true, aspect: "wide",
  },
  {
    id: "bts-02", src: "/media/gallery/G1_15_Coach_Clipboard.jpg",
    alt: "Coach with clipboard during timeout",
    title: "[The Clipboard — 2023]", event: "Atlanta Showout", year: 2023,
    category: "behind-the-scenes", aspect: "portrait",
  },
  {
    id: "bts-03", src: "/media/gallery/V_06_Coach_Huddle.jpg",
    alt: "Youth coach in a huddle with players",
    title: "[The Huddle — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "behind-the-scenes", aspect: "tall",
  },
  {
    id: "bts-04", src: "/media/gallery/G2_06_Coaching_Timeout.jpg",
    alt: "Coaching staff in a timeout discussion",
    title: "[The Timeout — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "behind-the-scenes", aspect: "landscape",
  },
  {
    id: "bts-05", src: "/media/gallery/G2_01_Coach_Portrait_Blue_Wall.jpg",
    alt: "Coach portrait against the blue wall before tip-off",
    title: "[The Read — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "behind-the-scenes", aspect: "portrait",
  },
  {
    id: "bts-06", src: "/media/gallery/V_03_Coach_Bench_Talk.jpg",
    alt: "Coach giving instructions to players on the bench",
    title: "[Bench Talk — 2024]", event: "Atlanta Showout", year: 2024,
    category: "behind-the-scenes", aspect: "tall",
  },
  {
    id: "bts-07", src: "/media/gallery/V_25_Facility_Check_In.jpg",
    alt: "Facial-recognition check-in at the venue entrance",
    title: "[Check-In — 2025]", event: "Super6 Showcase, Tampa", year: 2025,
    category: "behind-the-scenes", aspect: "landscape",
  },
  {
    id: "bts-08", src: "/media/gallery/V_14_Sideline_Coaches.jpg",
    alt: "Two coaches running the sideline mid-game",
    title: "[Sideline Calls — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "behind-the-scenes", aspect: "portrait",
  },
  {
    id: "bts-09", src: "/media/gallery/G1_04_Courtside_Coaching.jpg",
    alt: "Courtside coaching moment during a Super6 game",
    title: "[Courtside — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "behind-the-scenes", aspect: "landscape",
  },

  /* Venues & Facilities ─────────────────────────────── */
  {
    id: "vf-01", src: "/media/gallery/V_21_Crowd_Cheer.jpg",
    alt: "Crowd erupting in celebration during a Super6 game",
    title: "[The Eruption — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "venues-facilities", featured: true, aspect: "wide",
  },
  {
    id: "vf-02", src: "/media/gallery/G2_26_Crowd_Spectator_Multi.jpg",
    alt: "Multi-row crowd of spectators along the court",
    title: "[Multi-Row House — 2024]", event: "Atlanta Showout", year: 2024,
    category: "venues-facilities", featured: true, aspect: "ultra",
  },
  {
    id: "vf-03", src: "/media/gallery/V_19_Front_Row_Crowd.jpg",
    alt: "Front row of spectators packed along the floor",
    title: "[Front Row — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "venues-facilities", aspect: "landscape",
  },
  {
    id: "vf-04", src: "/media/gallery/V_15_Sideline_Spectators.jpg",
    alt: "Spectators clapping along the sideline",
    title: "[The Sideline — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "venues-facilities", aspect: "tall",
  },
  {
    id: "vf-05", src: "/media/gallery/V_23_Packed_Gym_Wide.jpg",
    alt: "Wide angle of the packed gym sideline",
    title: "[The Whole Wall — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "venues-facilities", aspect: "wide",
  },
  {
    id: "vf-06", src: "/media/gallery/V_17_Crowded_Sideline.jpg",
    alt: "Crowded sideline mid-tournament",
    title: "[Standing Room — 2024]", event: "Atlanta Showout", year: 2024,
    category: "venues-facilities", aspect: "landscape",
  },
  {
    id: "vf-07", src: "/media/gallery/G1_11_Packed_Crowd.jpg",
    alt: "Packed crowd in the stands",
    title: "[Sold Out — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "venues-facilities", aspect: "tall",
  },

  /* Athletes & Champions ─────────────────────────────── */
  {
    id: "ac-01", src: "/media/gallery/V_02_Trophy_Team_Banner.jpg",
    alt: "Youth champions holding trophy in front of Super6 banners",
    title: "[Champions — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "athletes-champions", featured: true, aspect: "wide",
  },
  {
    id: "ac-02", src: "/media/gallery/G1_02_Victory_Celebration.jpg",
    alt: "Team celebration on the court",
    title: "[The Celebration — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "athletes-champions", aspect: "portrait",
  },
  {
    id: "ac-03", src: "/media/gallery/V_09_Kings_Team_Portrait.jpg",
    alt: "Kings team portrait in purple and yellow uniforms",
    title: "[The Kings — 2024]", event: "Atlanta Showout", year: 2024,
    category: "athletes-champions", aspect: "landscape",
  },
  {
    id: "ac-04", src: "/media/gallery/G1_08_Team_Huddle_Celebration.jpg",
    alt: "Team huddle in celebration",
    title: "[Team Huddle — 2023]", event: "Atlanta Showout", year: 2023,
    category: "athletes-champions", aspect: "landscape",
  },
  {
    id: "ac-05", src: "/media/gallery/V_13_Kings_Squad.jpg",
    alt: "Squad of players in purple Kings gear posing together",
    title: "[Squad — 2023]", event: "Spring Classic, Orlando", year: 2023,
    category: "athletes-champions", aspect: "portrait",
  },
  {
    id: "ac-06", src: "/media/gallery/G2_13_Promotional_Portrait.jpg",
    alt: "Athlete promotional portrait",
    title: "[Player Portrait — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
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
    id: "bm-01", src: "/media/gallery/V_04_Montverde_Team_Banner.jpg",
    alt: "Montverde Academy team posed in front of Super6 banners",
    title: "[The Banner — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "brand-moments", featured: true, aspect: "wide",
  },
  {
    id: "bm-02", src: "/media/gallery/G2_07_Celebratory_Gesture_BKS.jpg",
    alt: "Celebratory gesture courtside in front of BKS partner branding",
    title: "[Partner Wall — 2024]", event: "Fall Tip-Off, Tampa", year: 2024,
    category: "brand-moments", aspect: "landscape",
  },
  {
    id: "bm-03", src: "/media/gallery/V_11_Two_Coaches_Banner.jpg",
    alt: "Two coaches standing in front of a Super6 brand wall",
    title: "[Brand Wall — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "brand-moments", aspect: "portrait",
  },
  {
    id: "bm-04", src: "/media/gallery/V_10_Family_Spectators.jpg",
    alt: "Family of spectators reacting on the bleachers",
    title: "[Family Energy — 2024]", event: "Atlanta Showout", year: 2024,
    category: "brand-moments", aspect: "landscape",
  },
  {
    id: "bm-05", src: "/media/gallery/V_12_Spectator_Peace_Signs.jpg",
    alt: "Young spectators flashing peace signs courtside",
    title: "[Courtside Cool — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "brand-moments", aspect: "tall",
  },
  {
    id: "bm-06", src: "/media/gallery/V_16_Kids_In_Stands.jpg",
    alt: "Three young fans in the stands wearing Super6 gear",
    title: "[The Next Wave — 2024]", event: "Spring Classic, Orlando", year: 2024,
    category: "brand-moments", aspect: "portrait",
  },
  {
    id: "bm-07", src: "/media/gallery/G2_04_Solo_Coach_With_Banner.jpg",
    alt: "Solo coach posed in front of the Super6 brand banner",
    title: "[The Coach — 2024]", event: "Super6 Showcase, Tampa", year: 2024,
    category: "brand-moments", aspect: "portrait",
  },
];

/* ─── Through the Years — one defining frame per year. All photos here
       are EXCLUSIVE to the timeline rail (not present in PHOTOS). ─── */
export type TimelineFrame = {
  year: number;
  yearLabel: string;
  src: string;
  caption: string;
  alt: string;
};

export const TIMELINE: TimelineFrame[] = [
  { year: 2014, yearLabel: "Year 1",  src: "/media/gallery/G1_28_Defensive_Stance.jpg",
    caption: "[The first whistle. One court, one weekend, one bracket.]", alt: "Charity Elite player squared up in a defensive stance" },
  { year: 2015, yearLabel: "Year 2",  src: "/media/uploads/timeline-2017-cavaliers-drive.png",
    caption: "[The standard set. Same brackets, sharper edges.]", alt: "Firehouse Cavaliers player driving to the basket at Super6" },
  { year: 2016, yearLabel: "Year 3",  src: "/media/gallery/G1_05_Crowd_Spectator.jpg",
    caption: "[Sidelines start to swell.]", alt: "Crowd along the sideline" },
  { year: 2017, yearLabel: "Year 4",  src: "/media/gallery/G1_29_Point_Guard.jpg",
    caption: "[New teams. New cities scouting in.]", alt: "Point guard handling the ball at midcourt" },
  { year: 2018, yearLabel: "Year 5",  src: "/media/gallery/G2_27_Youth_Defensive_Play.jpg",
    caption: "[First sold-out weekend.]", alt: "Kings player driving against defenders mid-game" },
  { year: 2019, yearLabel: "Year 6",  src: "/media/gallery/G1_13_Young_Spectators.jpg",
    caption: "[Expansion year. The schedule doubles.]", alt: "Young spectators watching the action" },
  { year: 2020, yearLabel: "Year 7",  src: "/media/gallery/G1_16_Timeout_Huddle.jpg",
    caption: "[Through the shutdown — and back.]", alt: "Timeout huddle during a game" },
  { year: 2021, yearLabel: "Year 8",  src: "/media/gallery/G2_10_Referee_Coach_Partnership.jpg",
    caption: "[The return. Louder than before.]", alt: "Referee and coach in partnership at the start of a Super6 game" },
  { year: 2022, yearLabel: "Year 9",  src: "/media/gallery/G1_21_Youth_Dribbling.jpg",
    caption: "[Multi-court weekends, full venues.]", alt: "Youth player dribbling in front of a Super6 banner" },
  { year: 2023, yearLabel: "Year 10", src: "/media/gallery/G2_08_Intense_Coaching_Moment.jpg",
    caption: "[Decade in. Officials certified across the bracket.]", alt: "Intense coaching moment courtside" },
  { year: 2024, yearLabel: "Year 11", src: "/media/gallery/S6_Good_Pic_01.jpg",
    caption: "[Trophies handed out. Stories made.]", alt: "Headline moment from a 2024 weekend" },
  { year: 2025, yearLabel: "Year 12", src: "/media/gallery/G2_11_Youth_Team_Promo.jpg",
    caption: "[Pro partnerships. League pass debuts.]", alt: "Youth team promotional moment" },
];

export const HERO_BACKGROUND = "/media/gallery/G1_19_Spectator_Group_7Women.jpg";

/* ─── Per-section theme. ─── */
export const SECTION_THEME: Record<GalleryCategory, "dark" | "cream"> = {
  "tournament-action":  "dark",
  "behind-the-scenes":  "dark",
  "venues-facilities":  "dark",
  "athletes-champions": "dark",
  "brand-moments":      "dark",
};

/* ─── Spotlight banners — full-bleed cinematic breaks between sections.
       These photos must NOT appear elsewhere on the gallery page. ─── */
export type Spotlight = {
  id: string;
  src: string;
  alt: string;
  eyebrow: string;
  quote: string;
  meta: string;
  placeAfter: GalleryCategory;
};

export const SPOTLIGHTS: Spotlight[] = [
  {
    id: "sp-01",
    src: "/media/gallery/G1_10_Youth_Lineup.jpg",
    alt: "Youth players lined up before a Super6 game",
    eyebrow: "12 Years In",
    quote:
      "What started as one weekend on one court is now a circuit. The kids who came to play are now coaching the kids who came to watch them.",
    meta: "Tampa · 2024",
    placeAfter: "tournament-action",
  },
  {
    id: "sp-02",
    src: "/media/gallery/G2_24_Coaching_Strategy_Session.jpg",
    alt: "Coaching strategy session courtside",
    eyebrow: "The Standard",
    quote:
      "We don't run weekends. We run rooms. Every entrance covered, every official certified, every minute on the clock earned.",
    meta: "The Super6 Way",
    placeAfter: "athletes-champions",
  },
];
