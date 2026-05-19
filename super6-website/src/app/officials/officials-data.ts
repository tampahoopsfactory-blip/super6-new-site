/* ─── /officials source of truth — Hiring Game Officials.
   Edit copy here. Page layout lives in page.tsx; CSS lives in globals.css.
   Naming and structure parallel src/app/rules/rules-data.ts. */

export const SIGNUP_URL =
  "https://docs.google.com/forms/d/e/1FAIpQLSd3r9iCxYdoHDCLSzLQpb2I4xu6ukgXIDaIV7tfQiLqsABSag/viewform?usp=header";

export type PayTier = {
  rate: string;
  unit: string;
  label: string;
  note: string;
};

export type ProgramFact = {
  label: string;
  value: string;
};

export const officialsIntro = {
  eyebrow: "Now Hiring",
  meta: "$20–$27 PER HOUR · WEEKLY GAMES",
  wordmarkPrimary: "NOW",
  wordmarkAccent: "HIRING.",
  wordmarkTag: "Game Officials",
  headline: "We're growing — and looking to expand our great team of referees.",
  desc:
    "Apply once and we'll book you weekly. Tuesday 7 PM assignments, immediate Sunday payouts, and a long-term path for officials who show up consistently and call clean ball.",
};

export const payTiers: PayTier[] = [
  {
    rate: "$20",
    unit: "/hr",
    label: "Non-Experienced Officials",
    note: "Building hours under our crew.",
  },
  {
    rate: "$25",
    unit: "/hr",
    label: "Non-Permanent Officials",
    note: "Game-by-game, available as needed.",
  },
  {
    rate: "$27",
    unit: "/hr",
    label: "Permanent Officials",
    note: "Long-term, consistent availability.",
  },
];

export const programFacts: ProgramFact[] = [
  { label: "Schedule", value: "Weekends only · Sat & Sun" },
  { label: "Booking Day", value: "Tuesdays at 7 PM" },
  { label: "Coverage", value: "Multi-city tournaments" },
  { label: "Payout", value: "Same-day Sunday" },
];

export const programPoints: string[] = [
  "Bookings are made every Tuesday at 7:00 PM.",
  "We are looking for qualified referees who can cover all of our games.",
  "Special consideration is given for long-term consistency in availability and quality of work.",
  "Referee consistency with Super6 Series LLC will secure court(s) for games long-term.",
  "If you are only looking for a few games from time-to-time, please still apply.",
  "Select only the city(s) and dates you are available.",
  "Must be available to work the entire event unless prior arrangements are established with Super6 Series LLC.",
];

export const finalCta = {
  eyebrow: "Ready to officiate?",
  title: "Submit your availability and join the rotation.",
  sub: "Bookings go out every Tuesday at 7 PM. Apply once — your selected cities and dates are how we assign games.",
};
