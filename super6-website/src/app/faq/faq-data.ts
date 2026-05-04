/* ─── FAQ source of truth
   Edit answers here. Markdown supported in `a` (bold, italics, links, lists).
   Slugs are stable — used for /faq#slug deep-links. Keep them URL-safe.

   Items marked `TODO_TK` are placeholders awaiting Tim Keeley confirmation. */

export type FaqItem = {
  q: string;
  /** Markdown string. Supports **bold**, *italic*, [links](https://…), - lists, 1. ordered lists, `code`. */
  a: string;
  /** Stable URL slug for deep-linking (#slug). Lowercase, kebab-case, unique across the page. */
  slug: string;
  /** When true, marks this item as needing TK review/confirmation. Renders a "TODO" pill in dev only. */
  pending?: boolean;
};

/** Lucide icon name (kebab-case) used to decorate the section header & nav. */
export type FaqIcon =
  | "sparkles"
  | "smartphone"
  | "calendar"
  | "book-open"
  | "shield-check"
  | "users"
  | "map-pin"
  | "cloud-rain";

export type FaqSection = {
  /** URL fragment id — also used for the sticky nav anchor. */
  id: string;
  /** Two-digit display number, e.g. "01". */
  number: string;
  /** Short label for the sticky nav and chip bar. */
  label: string;
  /** Long-form section title for the H2. */
  title: string;
  /** One-line section subtitle, optional. */
  description?: string;
  /** Lucide icon to render in the section header & nav. */
  icon: FaqIcon;
  items: FaqItem[];
};

export const faqIntro = {
  eyebrow: "Frequently Asked",
  title: "Asked & answered.",
  body:
    "Florida's longest-running youth basketball tournament. Boys and girls, 3rd–12th grade. 3–4 game minimum every event. Schedule drops Wednesdays at 8 PM in the Super6 app. Built directly on the rules at /rules — when in doubt, the rules page wins.",
};

export const faqSections: FaqSection[] = [
  {
    id: "getting-started",
    number: "01",
    label: "Getting Started",
    title: "Getting Started",
    description: "Registration, eligibility, and what every team needs to enter.",
    icon: "sparkles",
    items: [
      {
        slug: "cost-to-enter",
        q: "How much does it cost to enter a team?",
        a: `Two parts to every entry — a tiered registration fee plus a mandatory team fee.

**Registration (per team, per event):**

- **$99** if paid by the Sunday prior to the event
- **$125** after that Sunday's deadline
- **$195** after the schedule has been released

**Mandatory:** $25 security & officials fee for every team, every event. No exceptions.`,
      },
      {
        slug: "how-to-register",
        q: "How do I register my team?",
        a: `- Go to [thesuper6.com](https://www.thesuper6.com)
- Choose your event date
- Select your grade division
- Set up your team profile
- List your roster (players)
- Pay the registration

To update your roster after registering, log in to [basketball.exposureevents.com](https://basketball.exposureevents.com), go to **Dashboard** at the top, and edit.`,
      },
      {
        slug: "registration-deadline",
        q: "What is the registration deadline?",
        a: `The **$99** deadline is the Sunday before the event. After that, **$125**. After the schedule is released (Wednesday 8 PM), it jumps to **$195**. Pay early — it's a real discount.`,
      },
      {
        slug: "age-divisions",
        q: "What age cutoffs and divisions do you offer?",
        a: `**Boys and Girls, 3rd–12th grade.** We offer **D1**, **D2**, and **D3** brackets where team count supports it:

- **D1** — the highest level of competitive play.
- **D2** — a team that is forming and coming together.
- **D3** — a team that is developmental.

Players are placed by current school grade — verified through the school portal at check-in. Players may play **up** within the same club, never down.`,
      },
      {
        slug: "team-requirements",
        q: "What does my team need to provide to play?",
        a: `- Proof of team insurance (mandatory)
- Full matching uniforms with pressed numbers (no t-shirts)
- A team website or social media page
- Proof of previous tournament participation

Teams that can't meet these requirements will be disqualified. **No refund.**`,
      },
      {
        slug: "refund-policy",
        q: "Do you offer refunds?",
        a: `**No.** Under no circumstances do we offer refunds. This includes registration fees, gate badges, and any other purchase. Plan accordingly before you pay.`,
      },
    ],
  },

  {
    id: "app",
    number: "02",
    label: "The Super6 App",
    title: "The Super6 App",
    description: "Your single source of truth for schedules, brackets, and real-time updates.",
    icon: "smartphone",
    items: [
      {
        slug: "why-the-app",
        q: "Why do I need the Super6 app?",
        a: `The app is the **exclusive source** for event schedules, brackets, court assignments, and real-time updates. The website caches old data on some devices. The app pushes updates as they happen — that's why we built it.`,
      },
      {
        slug: "download-app",
        q: "Where do I download it?",
        a: `- **iOS (Apple):** App Store — search *Super6*
- **Android:** Google Play — search *Super6*`,
      },
      {
        slug: "schedule-release",
        q: "When does the schedule come out?",
        a: `Every **Wednesday at 8 PM** in the app. We don't release it earlier and we don't take requests for early team-participation info — fair, balanced matchups depend on us controlling that release.`,
      },
      {
        slug: "no-screenshots",
        q: "Why shouldn't I share screenshots of the schedule?",
        a: `Screenshots go stale within minutes — court changes, bracket reseeding, weather adjustments all happen in real time. **Always pull the schedule from the app**, not from a screenshot. It protects your team from showing up at the wrong court.`,
      },
      {
        slug: "app-login-issues",
        q: "I can't log in to the app — what now?",
        a: `Make sure you're using the same email you registered with. If your reset email doesn't arrive, check spam, then contact us via [Contact](/contact) and we'll resolve it same-day during the week.`,
        pending: true,
      },
      {
        slug: "push-notifications",
        q: "Do I need to enable push notifications?",
        a: `**Yes — strongly recommended.** Court changes, weather delays, and bracket updates push only through the app. If notifications are off, you may miss a tip-off change. Enable them under **Settings → Notifications → Super6** on iOS, or **App Info → Notifications** on Android.`,
        pending: true,
      },
    ],
  },

  {
    id: "schedule",
    number: "03",
    label: "Schedule & Format",
    title: "Schedule & Format",
    description: "How games, brackets, and game-day flow are structured.",
    icon: "calendar",
    items: [
      {
        slug: "divisions-offered",
        q: "What divisions do you offer?",
        a: `Boys and Girls, **3rd–12th grade**. **D1**, **D2**, and **D3** brackets where team count supports it:

- **D1** — the highest level of competitive play.
- **D2** — a team that is forming and coming together.
- **D3** — a team that is developmental.

For grade placement and play-up rules, see [What age cutoffs and divisions do you offer?](#age-divisions).`,
      },
      {
        slug: "game-guarantee",
        q: "What's the game guarantee?",
        a: `**3–4 games minimum** per team, per event.`,
      },
      {
        slug: "home-away",
        q: "How are home and away assigned?",
        a: `On the schedule, the **top team is Away**, the **bottom team is Home**.

- **Away team:** runs the clock (assistant coach)
- **Home team:** runs the game book (assistant coach)

If your team can't fill the book or clock spot, the game is forfeited. **No exceptions.**`,
      },
      {
        slug: "tiebreakers",
        q: "How do you break ties in pool play?",
        a: `Head-to-head result first. If still tied, **15-point differential** decides. If still tied, an automated coin flip via Exposure Events determines seeding.`,
      },
      {
        slug: "arrival-time",
        q: "What time should we arrive?",
        a: `**One hour before your scheduled game.** All teams must complete check-in before their 2nd game at the latest.`,
      },
      {
        slug: "running-late",
        q: "What if we're running late?",
        a: `Games start **5 minutes after** the scheduled time (or 5 minutes after the previous game ends, whichever is later). Teams not ready to play within that 5-minute window risk forfeiting the game.`,
      },
    ],
  },

  {
    id: "rules",
    number: "04",
    label: "Game Rules",
    title: "Game Rules",
    description: "NFHS standard. Highlights below — full rule book at [/rules](/rules).",
    icon: "book-open",
    items: [
      {
        slug: "game-length",
        q: "How long are the games?",
        a: `- **3rd–7th grade:** Two 16-minute halves, running clock
- **8th–12th grade:** Two 18-minute halves, running clock
- Clock stops at the **3-minute mark** before the end of the game
- If the score difference drops to **20 points**, the running clock starts again. If it drops back to **10 points**, the clock stops again.`,
      },
      {
        slug: "timeouts",
        q: "How many timeouts does each team get?",
        a: `**Four 30-second timeouts** per game. Overtime: one additional timeout in the first OT period and one additional in the second OT.`,
      },
      {
        slug: "overtime",
        q: "What's the overtime format?",
        a: `- **1st OT:** 1-minute period, plus one additional timeout per team
- **2nd OT:** sudden death — first team to score wins`,
      },
      {
        slug: "fouls-bonus",
        q: "What's the foul-out rule? Bonus / double-bonus?",
        a: `- **5 personal fouls** = player disqualified
- **7 team fouls** in a half = bonus (one free throw on every subsequent foul)
- **10 team fouls** in a half = double bonus (two free throws on every subsequent foul)`,
      },
      {
        slug: "ball-size",
        q: "What ball size do you use?",
        a: `- **3rd–5th grade (Boys & Girls):** 28.5" (size 6)
- **6th–8th grade Girls / 6th grade Boys:** 28.5" (size 6)
- **7th–12th grade Boys & 9th–12th grade Girls:** 29.5" (size 7)

Teams bring their own warm-up ball. The head referee selects a suitable game ball from the two teams.`,
        pending: true,
      },
      {
        slug: "shot-clock",
        q: "Is there a shot clock?",
        a: `**No shot clock** at any Super6 division. NFHS standard — full possession governed by closely-guarded and 10-second backcourt rules.`,
        pending: true,
      },
      {
        slug: "mercy-rule",
        q: "What's the mercy rule?",
        a: `When the score difference reaches **30 points or more**, running clock continues uninterrupted to the final buzzer (no clock stop in the final 3 minutes). If the lead drops back below 20, normal stop-clock rules resume.`,
        pending: true,
      },
    ],
  },

  {
    id: "checkin-gate",
    number: "05",
    label: "Check-In & Gate",
    title: "Check-In & Gate Security",
    description: "How players, coaches, and spectators get into the venue.",
    icon: "shield-check",
    items: [
      {
        slug: "who-pays-gate",
        q: "Who pays at the gate?",
        a: `Everyone except **uniformed players** and up to **two coaches per team**. Parents, family, friends, additional coaches — all pay. Badges are valid all day at the venue, **not per game**.`,
      },
      {
        slug: "gate-prices",
        q: "What are the gate prices?",
        a: `- **Friday:** Adults $9.95 / Kids $14.95
- **Saturday:** Adults $19.95 / Kids $9.95
- **Sunday:** Adults $19.95 / Kids $9.95
- **One-day-only events:** Adults $34.95 / Kids $17.95

No weekend passes. **All sales final. No refunds.**`,
      },
      {
        slug: "biometric-entry",
        q: "Do you use facial recognition or biometric entry?",
        a: `**Yes.** Super6 venues use a biometric check-in system at the gate to prevent badge swapping and ensure only paid spectators enter. First-time check-in takes ~10 seconds; re-entry on the same day is instant.`,
        pending: true,
      },
      {
        slug: "wristbands",
        q: "Do you issue wristbands?",
        a: `Wristbands are issued at the gate after payment and biometric check-in. Wristbands must be visible at all times inside the venue. **Wristband tampering = removal and athlete barred from the weekend.**`,
        pending: true,
      },
      {
        slug: "spectator-passes",
        q: "Are there spectator passes for the whole weekend?",
        a: `**No weekend passes.** Each day is purchased separately at the rates above.`,
      },
      {
        slug: "reentry",
        q: "Can spectators leave and come back the same day?",
        a: `**Yes — same-day re-entry is allowed** with a valid wristband. Biometric check confirms identity on re-entry. No re-entry across days without a new ticket.`,
        pending: true,
      },
      {
        slug: "sneak-in",
        q: "What happens if someone tries to sneak in or swap badges?",
        a: `- Unauthorized entry, badge swapping, or side-door entry = **immediate removal** by security
- The associated athlete is barred from that weekend's event
- **3 violations** from a team's spectators = entire team is expelled, all games forfeited, no refund

**We enforce this.** Tell your families before they show up.`,
      },
    ],
  },

  {
    id: "officials-coaches",
    number: "06",
    label: "Officials & Coaches",
    title: "Officials & Coaches",
    description: "Hiring referees, coach credentials, and conduct expectations.",
    icon: "users",
    items: [
      {
        slug: "hiring-officials",
        q: "How do I sign up to officiate Super6?",
        a: `Visit [/officials](/officials) for the full program — pay tiers, schedule, and the application form. We staff weekends only (Saturday & Sunday) and book on Tuesdays at 7 PM.`,
      },
      {
        slug: "coach-credentials",
        q: "What credentials do coaches need?",
        a: `Coaches must wear **collared shirts or team apparel** on the bench. Coaches must be listed on the team profile in Exposure Events. Background-check or certification requirements vary by venue — check the event page before traveling.`,
        pending: true,
      },
      {
        slug: "uniform-requirements",
        q: "What are the uniform requirements?",
        a: `**Strict matching only — no exceptions.**

- **Home team:** white or light-colored uniforms (listed at the bottom on the schedule)
- **Away team:** dark-colored uniforms (listed at the top on the schedule)
- Pressed numbers required on home, away, and reversible sets — front and back, no shared numbers
- **No T-shirts.** T-shirts are not acceptable uniforms.
- Coaches: collared shirts or team apparel

Referees check uniforms before every game. Non-compliance means the athlete cannot play. A team that doesn't follow uniform rules may be assessed a technical foul before tip-off.`,
      },
      {
        slug: "jerseys-printing",
        q: "What if our jerseys are still being printed?",
        a: `**Don't show up.** Seriously. We will not make exceptions on the uniform rule, and we'd rather you skip the event than burn your $25 fee at the door.`,
      },
      {
        slug: "conduct-policy",
        q: "What's the conduct policy?",
        a: `We have a **strict zero-tolerance conduct policy.** Disrespect toward officials, staff, or other teams = immediate ejection from the venue/tournament and possible permanent ban from future Super6 events.

If a parent, relative, or friend of an athlete is the violator, the associated athlete is barred from that weekend's event.`,
      },
      {
        slug: "ejection-rules",
        q: "What happens if a player or coach is ejected?",
        a: `An ejected player or coach must leave the playing area immediately and is **suspended from the team's next Super6 game**. A second ejection in the same season triggers a Super6 director review and possible permanent ban. **Fees and forfeits apply per the forfeit policy.**`,
        pending: true,
      },
      {
        slug: "eligibility-challenge",
        q: "Can I challenge another team's player eligibility?",
        a: `Yes. Challenge fee is **$100 per athlete challenged**.

- If proper documentation isn't provided, that player can't participate
- If a game has already been played and the player is found ineligible, the team forfeits that game
- If the challenge is upheld (player ineligible), the **$100 fee is refunded** to the challenger`,
      },
    ],
  },

  {
    id: "venues",
    number: "07",
    label: "Venues & Logistics",
    title: "Venues & Logistics",
    description: "Where we play, parking, food, and travel partners.",
    icon: "map-pin",
    items: [
      {
        slug: "venue-locations",
        q: "Where are Super6 events held?",
        a: `Super6 runs across **Florida and Georgia**:

- **Orlando** — primary hub
- **Tampa**
- **Clearwater**
- **West Palm Beach**
- **Atlanta, GA**

Specific venue addresses post on each event listing once confirmed. See [/locations](/locations) for the full season map.`,
      },
      {
        slug: "parking",
        q: "Is there parking at the venues?",
        a: `**Yes — all venues include on-site or adjacent parking.** Parking details are included in your pre-event email and posted in the app the week of the event. Arrive 60 minutes before tip-off to allow for parking, check-in, and warm-up.`,
        pending: true,
      },
      {
        slug: "food-policy",
        q: "Can we bring outside food and drinks?",
        a: `**No outside food or drinks** at most Super6 venues. Each venue has on-site concessions with team-friendly pricing. Sealed water bottles for athletes are allowed; check the event page for venue-specific exceptions.`,
        pending: true,
      },
      {
        slug: "warmup-ball",
        q: "Do we bring our own basketball?",
        a: `**Yes — for warmups.** Super6 does not provide warm-up balls. The head referee selects a suitable game ball from the two teams.`,
      },
      {
        slug: "hotel-discounts",
        q: "Do you have hotel discounts for traveling teams?",
        a: `**Yes.** Discounted room blocks are available at [thesuper6.com/copy-of-hotels-orl-1](/copy-of-hotels-orl-1). Book early — rates and availability are limited.`,
      },
    ],
  },

  {
    id: "refunds-weather",
    number: "08",
    label: "Refunds & Weather",
    title: "Refunds, Weather & Disputes",
    description: "Cancellations, weather delays, forfeits, and how to escalate.",
    icon: "cloud-rain",
    items: [
      {
        slug: "refunds-final",
        q: "Are refunds ever issued?",
        a: `**No.** Under no circumstances do we offer refunds — registration fees, gate badges, security & officials fees, and any other purchase are all final. Plan accordingly.`,
      },
      {
        slug: "forfeit-policy",
        q: "What happens if a team forfeits a game?",
        a: `- **$100 forfeit fee** must be paid before that team plays its next game in the event
- Plus a **$100 forfeit deposit** required before playing in any future Super6 event
- We can't guarantee opposing-team attendance — make sure your scheduled opponent is present before tip-off

**No exceptions to this policy.**`,
      },
      {
        slug: "weather-delays",
        q: "What happens if there's a weather delay or cancellation?",
        a: `Outdoor and indoor venues alike are subject to weather delays. Updates push through the **Super6 app** in real time. If a session is delayed, the schedule reseats automatically and your team's next game time updates in the app. **No refunds for weather** — events that begin and are then cut short by weather are considered played.`,
        pending: true,
      },
      {
        slug: "dispute-resolution",
        q: "How do I dispute a result, ruling, or fee?",
        a: `On-court rulings are **final** at the moment they're called. Post-event disputes (eligibility, forfeit fees, conduct decisions) go through the Super6 director. Email through the [Contact page](/contact) with your team name, event date, and a clear summary. We respond same-day during the week.`,
        pending: true,
      },
      {
        slug: "contact-escalation",
        q: "Who do I contact if I still have questions?",
        a: `- **Schedule, court, real-time event questions:** the Super6 app
- **Roster updates:** [basketball.exposureevents.com](https://basketball.exposureevents.com) → Dashboard → Edit Roster
- **Registration, billing, anything else:** [Contact Super6](/contact)

We respond same-day during the week.`,
      },
    ],
  },
];

/** Total number of questions across all sections (for the hero index). */
export const totalQuestions = faqSections.reduce(
  (acc, s) => acc + s.items.length,
  0
);

/** Generate FAQPage JSON-LD schema from the data. Strips markdown to plain text for the answer field. */
export function buildFaqJsonLd() {
  const stripMd = (s: string): string =>
    s
      .replace(/\*\*(.+?)\*\*/g, "$1")
      .replace(/\*(.+?)\*/g, "$1")
      .replace(/\[(.+?)\]\((.+?)\)/g, "$1")
      .replace(/^[\s>*-]+/gm, "")
      .replace(/^\d+\.\s+/gm, "")
      .replace(/`([^`]+)`/g, "$1")
      .replace(/\n{2,}/g, " ")
      .replace(/\n/g, " ")
      .trim();

  return {
    "@context": "https://schema.org",
    "@type": "FAQPage",
    mainEntity: faqSections.flatMap((section) =>
      section.items.map((item) => ({
        "@type": "Question",
        name: item.q,
        acceptedAnswer: {
          "@type": "Answer",
          text: stripMd(item.a),
        },
      }))
    ),
  };
}
