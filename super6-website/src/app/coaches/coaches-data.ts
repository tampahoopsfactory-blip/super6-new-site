/* ─── /coaches source of truth — Coaches appreciation + recruitment.
   Edit copy here. Page layout lives in page.tsx; CSS lives in globals.css.
   Naming and structure parallel src/app/rules/rules-data.ts. */

import type { RuleIcon } from "../rules/rules-data";

export type CoachStatement = {
  /** Two-digit display number, e.g. "01". */
  num: string;
  /** Statement body. Plain text only — no markdown for this static page. */
  body: string;
};

export type CoachPillar = {
  /** Short uppercase tag, e.g. "AFFORDABLE". */
  tag: string;
  /** Single-paragraph body. */
  body: string;
};

export type CoachSection = {
  /** URL fragment id — also used by sections for in-page jumps. */
  id: string;
  /** Two-digit display number, e.g. "01". */
  number: string;
  /** Section H2 title. */
  title: string;
  /** One-line italic subtitle (optional). */
  description?: string;
  /** Lucide icon name — reuses the RuleIcon registry from /rules. */
  icon: RuleIcon;
  /** Optional section photo shown beside the section body. */
  image?: {
    src: string;
    alt: string;
    /**
     * Source file includes baked-in letterboxing (often uneven bottom bar).
     * Applies zoom + focal shift so the crop fills the column and clips bars.
     */
    cropLetterbox?: boolean;
  };
};

/* ─── Hero copy ─── */
export const coachesIntro = {
  eyebrow: "Coaches",
  meta: "FOR THE COACHES",
  wordmarkTop: "TO THE",
  wordmarkAccent: "coaches",
  wordmarkLabel: "who do everything",
  headline: "We see you.",
  desc:
    "You're the ride. The mentor. The fundraiser. The reason a kid eats. The reason a kid plays. Coaching ends at the buzzer. The job doesn't.",
  index: {
    num: "12",
    label: "Years built around the coach",
    sub: "Affordable tournaments. High-caliber competition. Schedule density that keeps teams together longer.",
  },
};

/* ─── Section 01 — The Job Description Nobody Wrote ─── */
export const jobDescriptionSection: CoachSection = {
  id: "job-description",
  number: "01",
  title: "The job description nobody wrote.",
  description: "Coaching ends at the buzzer. The job doesn't.",
  icon: "user-check",
  image: {
    src: "/media/uploads/coaches-section01-coach-player-sideline.png",
    alt: "Coach talking with a youth player on the sideline during a game",
    cropLetterbox: true,
  },
};

export const jobDescriptionItems: CoachStatement[] = [
  { num: "01", body: "You pick up kids when no one else can." },
  { num: "02", body: "You feed players when home is running empty." },
  {
    num: "03",
    body: "You make sure the kid with nowhere to sleep has a place to sleep.",
  },
  {
    num: "04",
    body: "You coach through losses, bad grades, broken homes, and long nights.",
  },
  { num: "05", body: "Then you still have to run practice and coach games." },
];

/* ─── Section 02 — The Bill Nobody Talks About ─── */
export const billSection: CoachSection = {
  id: "the-bill",
  number: "02",
  title: "The bill nobody talks about.",
  description:
    "Team dues never cover the whole season. So coaches cover the gap. Out of pocket. Quietly. Every year.",
  icon: "clipboard-list",
  image: {
    src: "/media/uploads/coaches-section02-coach-players-galaxy.png",
    alt: "Coach talking with youth basketball players on the court",
  },
};

export const billItems: CoachStatement[] = [
  { num: "01", body: "Gym time." },
  { num: "02", body: "Tournament fees." },
  { num: "03", body: "Uniforms." },
  { num: "04", body: "Travel." },
  { num: "05", body: "Equipment." },
  { num: "06", body: "Refs." },
  { num: "07", body: "Subs." },
];

/* ─── Section 03 — The Cut That Hurts the Most ─── */
export const cutSection: CoachSection = {
  id: "the-cut",
  number: "03",
  title: "The cut that hurts the most.",
  icon: "scale",
  image: {
    src: "/media/uploads/coaches-section03-cut-coach-gym.jpg",
    alt: "Coach in a basketball gym wearing team apparel near the court",
  },
};

export const cutBody = [
  "You spend a year, sometimes two, building a kid up. Footwork. IQ. Character. Confidence.",
  "The day he turns into the player you knew he could be, somebody else recruits him.",
  "You don't get paid. You get poached.",
  "We know. We've watched it happen for years.",
];

/* ─── Section 04 — Super6 Series LLC Was Built Different ─── */
export const builtDifferentSection: CoachSection = {
  id: "built-different",
  number: "04",
  title: "Super6 was built different.",
  description:
    "We built this league for the coach who shows up. Who stays. Who gives a damn.",
  icon: "trophy",
  image: {
    src: "/media/uploads/coaches-section04-coach-players-huddle.png",
    alt: "Coach addressing youth basketball players on the court",
  },
};

export const pillars: CoachPillar[] = [
  {
    tag: "Affordable",
    body:
      "Entry fees that don't punish you for fielding a team. Coaches keep more. Programs survive longer.",
  },
  {
    tag: "High quality",
    body:
      "Same caliber teams as the expensive circuits. Same level of competition. A fraction of the cost.",
  },
  {
    tag: "Deep variety",
    body:
      "Teams from across the country and across borders. Out-of-state and international squads come for the value and stay for the experience.",
  },
];

/* ─── Numbers strip ─── */
export const numbers: { value: string; label: string }[] = [
  { value: "400+", label: "Tournaments run" },
  { value: "12", label: "Years doing this" },
  {
    value: "10–15",
    label: "Tournaments a season a Super6 coach can afford",
  },
  {
    value: "4",
    label:
      "Host cities — Clearwater · Orlando · Tampa · Boca Raton (Atlanta coming)",
  },
];

/* ─── Section 05 — Why Coaches Keep Coming Back ─── */
export const wordOfMouthSection: CoachSection = {
  id: "word-of-mouth",
  number: "05",
  title: "Why coaches keep coming back.",
  icon: "calendar-clock",
  image: {
    src: "/media/uploads/coaches-section05-team-bleachers.png",
    alt: "Youth basketball teammates posing together on gym bleachers",
  },
};

export const wordOfMouthBody = [
  "Coaches play one Super6 weekend, then come back next season with more teams.",
  "After that, they go home and tell other coaches in their state. That's how this league grew.",
  "We don't need a sales pitch. The coaches already in the gym do that for us.",
];

/* ─── Thesis — Keep the team. Keep the kid. ─── */
export const thesis = {
  headline: "Keep the team. Keep the kid.",
  stair: [
    "Affordable tournaments mean longer seasons.",
    "Longer seasons mean more time with your players.",
    "More time means more mentorship.",
    "More mentorship is how lives change.",
  ],
  close:
    "Super6 doesn't just run events. We help coaches stay in their kids' lives.",
  images: [
    {
      src: "/media/uploads/coaches-thesis-kids-tacticboard.png",
      alt: "Youth players reviewing a tactics board together",
    },
    {
      src: "/media/curated/25-coaches-clipboard.jpg",
      alt: "Coach leading players from the sideline",
    },
  ],
};

/* ─── Final CTA ─── */
export const finalCta = {
  eyebrow: "For the coaches who stay",
  title: "Bring your team. Play a full season. Keep them together.",
  sub: "Questions about pricing, schedules, or hosting? We answer fast.",
};
