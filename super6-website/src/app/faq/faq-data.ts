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
    "Florida's longest-running youth basketball tournament. Boys and girls, 3rd–12th grade. Three-game minimum every event. Schedule drops Wednesdays at 8 PM in the Super6 app. Built directly on the rules at /rules — when in doubt, the rules page wins.",
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
        a: `- **Two parts:** tiered registration fee + mandatory team fee
- **Registration (per team, per event):**
  - **$99** — paid by the **Sunday prior** to the event
  - **$125** — after that Sunday's deadline
  - **$195** — after the schedule has been released
- **Mandatory:** **$25** security & officials fee — **every team, every event** — no exceptions`,
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
- **Roster updates after you register:** log in to [basketball.exposureevents.com](https://basketball.exposureevents.com) → **Dashboard** (top) → edit`,
      },
      {
        slug: "registration-deadline",
        q: "What is the registration deadline?",
        a: `- **$99** — pay by the **Sunday before** the event
- **$125** — after that Sunday's deadline
- **$195** — after the schedule is released (**Wednesday 8 PM** in the app)
- Pay early — it's a real discount`,
      },
      {
        slug: "age-divisions",
        q: "What grade divisions do you offer?",
        a: `- **We are not an age-based organization** — we are a **grade-based** organization
- **An athlete must** be in the **appropriate grade** to play in that **grade division**
- **Boys and Girls, 3rd–12th grade**
- **D1**, **D2**, and **D3** brackets where team count supports it:
  - **D1** — the highest level of competitive play
  - **D2** — a team that is forming and coming together
  - **D3** — a team that is developmental
- **Grade placement:** current school grade — verified through the **school portal** at check-in
- **Play up:** allowed within the same club — **never down**`,
      },
      {
        slug: "team-requirements",
        q: "What does my team need to provide to play?",
        a: `- **Team insurance (mandatory):** each team **must carry its own** coverage and provide **proof** — Super6 **does not** insure your team or substitute for your program's coverage
- **Liability:** Super6 is **not responsible or liable** for injury, loss, damage, or other harm to **athletes, spectators, patrons**, or anyone else in connection with our events — **your team and your guests attend at your own risk**
- **When you sign up for an event:** you **consent** that **you are responsible for your party** — including **parents, friends, athletes, coaches**, and anyone you bring, register, or represent — **not Super6**
- Full matching uniforms with pressed numbers (**no t-shirts**)
- A team website or social media page
- Proof of previous tournament participation
- **Can't meet the list?** disqualified — **no refund**`,
      },
      {
        slug: "refund-policy",
        q: "Do you offer refunds?",
        a: `- **Default: no** — we do **not** offer refunds under normal circumstances
- **Only exception:** you **paid team registration** for an event **in your division**, and that **division is not played** because **Super6 cannot field enough teams** to make the bracket — we send an **immediate refund**. **We do not hold your money**
- **No refunds on:** gate badges, security & officials fees, **any other purchase**, or team registration when your division **does** run — **plan before you pay**`,
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
        a: `- **Super6 runs the event through the app** — schedules, brackets, court assignments, and every change made **during the weekend** land there first and stay **current**
- **We strongly urge coaches, parents, and players to use the app — not the website** — for anything time- or location-sensitive
- **The website can cache on phones** — you may see **old** brackets, times, or courts while the app already shows the **live** state. Do **not** treat the public site as a real-time feed
- **If you rely on the website and miss a game, miss a tip time, or show up on the wrong schedule because the copy on your device was out of date, Super6 is not responsible** — the app is the official channel; use it`,
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
        a: `- **Every Wednesday at 8 PM** — in the app only
- **No early release** — we don't take requests for early team info
- **Why:** fair, balanced matchups depend on controlling that release`,
      },
      {
        slug: "no-screenshots",
        q: "Why shouldn't I share screenshots of the schedule?",
        a: `- Screenshots go **stale within minutes** — courts, brackets, and weather shift in real time
- **Always pull from the app** — never trust a screenshot as current
- **Protects your team** from showing up at the wrong court`,
      },
      {
        slug: "app-login-issues",
        q: "I can't log in to the app — what now?",
        a: `- **Most Super6 information does not require a login** — you **do not need** login credentials to access schedules and general updates in the app. **Download** the Super6 app and **use it**
- **Follow your team:** when you **follow your team** in the app, you'll get **automatic text messages (SMS)** from the app for anything relevant to **that team** — schedule releases, changes, and other updates`,
      },
      {
        slug: "push-notifications",
        q: "Do I need to enable push notifications?",
        a: `- **Yes — strongly recommended**
- Courts, weather, and bracket updates **push through the app only**
- Notifications **off** = you can miss a tip-off change
- **iOS:** **Settings → Notifications → Super6**
- **Android:** **App Info → Notifications**`,
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
        a: `- **We are not an age-based organization** — we are a **grade-based** organization
- **An athlete must** be in the **appropriate grade** to play in that **grade division**
- **Boys and Girls, 3rd–12th grade**
- **D1**, **D2**, and **D3** brackets where team count supports it:
  - **D1** — the highest level of competitive play
  - **D2** — a team that is forming and coming together
  - **D3** — a team that is developmental
- **Grade placement + play up:** [What grade divisions do you offer?](#age-divisions)`,
      },
      {
        slug: "game-guarantee",
        q: "What's the game guarantee?",
        a: `- **3 games minimum** per team, per event`,
      },
      {
        slug: "home-away",
        q: "How are home and away assigned?",
        a: `- On the schedule: **top = Away**, **bottom = Home**
- **Away team:** runs the clock (assistant coach)
- **Home team:** runs the game book (assistant coach)
- **Can't fill book or clock?** game is forfeited — **no exceptions**`,
      },
      {
        slug: "tiebreakers",
        q: "How do you break ties in pool play?",
        a: `- **1.** Head-to-head result
- **2.** Still tied? **15-point differential**
- **3.** Still tied? Automated coin flip via **Exposure Events**`,
      },
      {
        slug: "arrival-time",
        q: "What time should we arrive?",
        a: `- **Arrive:** one hour before your scheduled game
- **Check-in deadline:** before your **2nd game** at the latest`,
      },
      {
        slug: "running-late",
        q: "What if your team is running late?",
        a: `**If your team is running late:**

- Games start **5 minutes after** the scheduled time, **or** 5 minutes after the previous game ends — **whichever is later**.
- Teams not ready to play within that **5-minute window** risk **forfeiting the game**.`,
      },
    ],
  },

  {
    id: "rules",
    number: "04",
    label: "Rules & Eligibility",
    title: "Rules & Eligibility",
    description: "Grade verification at check-in, eligibility challenges, on-court NFHS highlights, and the full rule book at [/rules](/rules).",
    icon: "book-open",
    items: [
      {
        slug: "check-in-process-team-book",
        q: "Tell me about your check-in process. Do I need to have my team book?",
        a: `- **No team book** — Super6 does **not** use traditional paper team books at check-in
- **School portal on the athlete's own device:** each player must be ready to log in to their **school portal** on their **personal phone**, **away from coaches and parents**, with a **Super6 site manager** present
- **What we're proving:** the athlete is in the **correct grade** for the division / event they entered
- **If a player cannot complete verification:** any games that athlete **already played** in the tournament may be **ruled forfeits**
- **Athlete review during play:** site managers, referees, and staff watch for players who **appear above grade**, look **materially more developed** than peers, or **dominate** in ways that don't match the division — same standard as check-in: keep the bracket honest and catch **bad actors** early
- **Why it matters:** strong check-in and in-game monitoring cut down on late arrivals skipping verification and on programs trying to slip ineligible players onto the floor`,
      },
      {
        slug: "eligibility-challenge",
        q: "How do I challenge an athlete I think is older or in the wrong division?",
        a: `- **Anyone** may challenge — there is **no restriction** on who can file a challenge
- **$100 challenge fee** per athlete challenged — paid by the challenger via **Cash App or Venmo only** (**not** credit card)
- **Site director** conducts a **full check** with **only the athlete** present — **no coaches, no parents, no one else** in the room
- **Athlete must** in **real time** log in to **their school portal** and show proof they are in the **correct grade** for that division at that moment
- **Site director** may ask **specific questions** (current grade, teachers, classes, prior grades, etc.) to confirm the athlete belongs in that division
- **If the athlete fails** the challenge or **cannot** complete verification — **all games** that athlete played **or would play** in **that division** at this event are **forfeited** (past and future)
- **Challenge upheld** (player ruled ineligible): **$100 refunded** to the challenger`,
      },
      {
        slug: "game-length",
        q: "How long are the games?",
        a: `- **3rd–7th grade:** two **16-minute** halves, running clock
- **8th–12th grade:** two **18-minute** halves, running clock
- Clock **stops** at the **3-minute mark** before the end of regulation
- **Spread hits 20:** running clock **on**
- **Spread back to 10:** clock **stops** again (normal rules)`,
      },
      {
        slug: "timeouts",
        q: "How many timeouts does each team get?",
        a: `- **Regulation:** **four** 30-second timeouts per game
- **1st OT:** one **additional** timeout per team
- **2nd OT:** one **additional** timeout per team`,
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
        a: `- **3rd–5th (Boys & Girls):** 28.5" (size 6)
- **6th–8th Girls / 6th Boys:** 28.5" (size 6)
- **7th–12th Boys & 9th–12th Girls:** 29.5" (size 7)
- **Warm-up:** teams bring their own ball
- **Game ball:** head referee picks from the two teams' balls`,
        pending: true,
      },
      {
        slug: "shot-clock",
        q: "Is there a shot clock?",
        a: `- **No shot clock** at any Super6 division
- **NFHS standard** — possession governed by closely-guarded and 10-second backcourt rules`,
        pending: true,
      },
      {
        slug: "mercy-rule",
        q: "What's the mercy rule?",
        a: `- **Lead ≥ 30:** running clock runs straight to the buzzer (**no** stop in the final 3 minutes)
- **Lead drops below 20:** normal stop-clock rules **resume**`,
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
        a: `- **No charge:** uniformed players + up to **two coaches** per team
- **Everyone else pays:** parents, family, friends, extra coaches
- **Badges:** good **all day** at the venue — **not** per game`,
      },
      {
        slug: "gate-prices",
        q: "What are the gate prices?",
        a: `- **Friday:** Adults $9.95 / Kids $14.95
- **Saturday:** Adults $19.95 / Kids $9.95
- **Sunday:** Adults $19.95 / Kids $9.95
- **One-day-only events:** Adults $34.95 / Kids $17.95
- **No weekend passes**
- **All sales final — no refunds**`,
      },
      {
        slug: "biometric-entry",
        q: "Do you use facial recognition or biometric entry?",
        a: `- **Yes** — biometric check-in at the gate
- **Why:** stops badge swapping; only **paid** spectators get in
- **First check-in:** ~**10 seconds**
- **Same-day re-entry:** **instant** after you're enrolled`,
        pending: true,
      },
      {
        slug: "wristbands",
        q: "Do you issue wristbands?",
        a: `- Issued at the gate **after** payment + biometric check-in
- Must stay **visible** at all times inside the venue
- **Tampering** = removal + **athlete barred** for the weekend`,
        pending: true,
      },
      {
        slug: "spectator-passes",
        q: "Are there spectator passes for the whole weekend?",
        a: `- **No weekend passes**
- **Each day** purchased separately (see gate prices above)`,
      },
      {
        slug: "reentry",
        q: "Can spectators leave and come back the same day?",
        a: `- **Yes** — same-day re-entry with a **valid wristband**
- **Biometric** confirms identity on the way back in
- **Different day?** need a **new** ticket — no cross-day re-entry`,
        pending: true,
      },
      {
        slug: "sneak-in",
        q: "What happens if someone tries to sneak in or swap badges?",
        a: `- **Sneak / swap / side door** = **immediate removal** by security
- **Athlete tied to the violator** = barred for that weekend's event
- **3 spectator violations** from one team = **whole team** out, all games forfeited, **no refund**
- **We enforce this** — tell families **before** they arrive`,
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
        a: `- **Start here:** [/officials](/officials) — pay tiers, schedule, application
- **When we staff:** weekends only (**Sat & Sun**)
- **Booking:** **Tuesdays at 7 PM**`,
      },
      {
        slug: "coach-credentials",
        q: "What credentials do coaches need?",
        a: `- **Bench dress:** collared shirts **or** team apparel
- **Roster:** coaches listed on the team profile in **Exposure Events**
- **Background checks / certs:** **vary by venue** — read the **event page** before you travel`,
        pending: true,
      },
      {
        slug: "uniform-requirements",
        q: "What are the uniform requirements?",
        a: `- **Strict matching only** — no exceptions
- **Home (bottom on schedule):** white or **light** uniforms
- **Away (top on schedule):** **dark** uniforms
- **Numbers:** pressed on home, away, and reversibles — **front and back** — **no** duplicate numbers on one roster
- **No T-shirts** — not acceptable as a uniform
- **Coaches:** collared shirt **or** team apparel
- **Pre-game:** refs check every game — **out of uniform = can't play**
- **Team not in compliance** may be assessed a **technical foul** before tip-off`,
      },
      {
        slug: "jerseys-printing",
        q: "What if our jerseys are still being printed?",
        a: `- **Don't show up** without legal uniforms
- **Zero exceptions** on the uniform rule
- We'd rather you **skip the weekend** than **lose the $25** security & officials fee at the door for nothing`,
      },
      {
        slug: "conduct-policy",
        q: "What's the conduct policy?",
        a: `- **Zero tolerance** for disrespect toward officials, staff, or other teams
- **Penalty:** immediate ejection from the venue/tournament + possible **permanent ban**
- **Parent / relative / friend** is the problem? **That athlete** is barred for the weekend`,
      },
      {
        slug: "ejection-rules",
        q: "What happens if a player or coach is ejected?",
        a: `- **Leave the floor now** — no debate
- **Next Super6 game:** automatic **suspension**
- **Second ejection same season:** director review + possible **permanent ban**
- **Money:** fees & forfeits follow the **forfeit policy**`,
        pending: true,
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
        a: `- **Markets:** **Florida** + **Georgia**
- **Orlando** — primary hub
- **Tampa**
- **Clearwater**
- **West Palm Beach**
- **Atlanta, GA**
- **Addresses:** drop on each event listing once the building is locked
- **Map:** [/locations](/locations)`,
      },
      {
        slug: "parking",
        q: "Is there parking at the venues?",
        a: `- **Yes** — on-site **or** walkable adjacent parking at every building we use
- **Details:** pre-event email + app (**week of** the tournament)
- **Plan:** be parked **60 minutes** before tip — leaves room for check-in + warm-ups`,
        pending: true,
      },
      {
        slug: "food-policy",
        q: "Can we bring outside food and drinks?",
        a: `- **Outside food / drinks:** **not allowed** at most venues
- **Concessions:** on-site with team-friendly pricing
- **Athletes:** sealed **water** is usually OK — read the **event page** for that gym's rules`,
        pending: true,
      },
      {
        slug: "warmup-ball",
        q: "Do we bring our own basketball?",
        a: `- **Yes** — bring your own for **warmups**
- **Super6 does not** supply warm-up balls
- **Game ball:** referee picks from **your two** game balls`,
      },
      {
        slug: "hotel-discounts",
        q: "Do you have hotel discounts for traveling teams?",
        a: `- **Yes** — discounted room blocks live here: [Hotel blocks](/copy-of-hotels-orl-1)
- **Book early** — price and inventory **go fast**`,
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
        a: `- **Default: no** — same policy as [Do you offer refunds?](#refund-policy) in Getting Started
- **Only exception:** **paid team registration** for a division that **Super6 cancels** because **not enough teams** register to run it — **immediate refund**. **We do not hold your money**
- **Otherwise:** **registration** (when your division runs), **gate**, **security & officials**, and **everything else** — **no refunds** — **decide before you pay**`,
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
        a: `- **Any venue** — indoor or outdoor — can be affected by severe weather
- **Updates:** real-time, **Super6 app only**
- **If play is delayed:** bracket **reseats automatically** — your next game time updates in the app
- **Refunds:** **none** for weather
- **If the event starts** and is then stopped by weather, it is **treated as played**`,
        pending: true,
      },
      {
        slug: "dispute-resolution",
        q: "How do I dispute a result, ruling, or fee?",
        a: `- **On the court:** rulings are **final** when made — no re-argument at the scorer's table
- **After the event:** eligibility, forfeit fees, conduct → **Super6 director**
- **How to escalate:** email via [Contact](/contact) — include **team name**, **event date**, and a clear summary
- **Response time:** same-day during the **work week**`,
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
