/* ─── /rules source of truth — Super 6 official rule book.
   Edit rule bodies here. Markdown supported in `body` (bold, italics, links, lists).
   Slugs are stable — used for /rules#slug deep-links. Keep them URL-safe.

   Authored from the original /rules/page.tsx (commit 6276cc9). Content is
   identical; structure was lifted into typed sections so the FAQ editorial
   pattern (sticky rail, scroll-spy, single-open accordion, deep-link expand)
   can be reused verbatim from src/app/faq/_components/. */

export type RuleItem = {
  /** Rule title — short, prescriptive. Renders as the accordion trigger. */
  title: string;
  /** Markdown string. Supports **bold**, *italic*, [links](https://…), - lists, 1. ordered lists, `code`. */
  body: string;
  /** Stable URL slug for deep-linking (#slug). Lowercase, kebab-case, unique across the page. */
  slug: string;
  /** When true, marks this item as needing TK review/confirmation. Renders a "TODO" pill. */
  pending?: boolean;
};

/** Lucide icon name (kebab-case) used to decorate the section header & nav. */
export type RuleIcon =
  | "scale"
  | "shirt"
  | "clipboard-list"
  | "timer"
  | "trophy"
  | "user-check"
  | "calendar-clock";

export type RuleSection = {
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
  icon: RuleIcon;
  items: RuleItem[];
};

export const rulesIntro = {
  eyebrow: "Official",
  title: "Played by the book.",
  body:
    "Super 6 follows National Federation of High School Basketball standards. The rules below govern conduct, uniform compliance, game format, eligibility, and scheduling at every Super 6 weekend.",
};

export const rulesSections: RuleSection[] = [
  {
    id: "conduct",
    number: "01",
    label: "Conduct",
    title: "A respectful weekend, or no weekend.",
    description:
      "Admission, gate behavior, and how violations cascade from spectator to athlete to team.",
    icon: "scale",
    items: [
      {
        slug: "admission",
        title: "Admission",
        body: `Travel sports come with financial pressure on clubs. To keep events affordable and high-quality, **everyone except players in uniform and a maximum of two coaches per team** is required to pay venue admission.

See the [No-cash policy](#no-cash-policy) below — we do **not** accept cash at the gate.`,
      },
      {
        slug: "no-cash-policy",
        title: "No-cash gate policy",
        body: `**We do not accept cash at the gate.** Pay quickly and securely with **Cash App, Venmo, Apple Pay, Zelle, or any major credit card**. Have your method ready before you reach the front of the line.`,
      },
      {
        slug: "reporting-violations",
        title: "Reporting violations",
        body: `To uphold the integrity of our events, please report any violations of the admission or conduct policy to our team via the [Contact page](/contact). Reports are reviewed same-day during the week.`,
      },
      {
        slug: "badge-exchange",
        title: "Badge exchange & unauthorized entry",
        body: `Anyone caught exchanging badges or attempting to enter the venue through side doors, exits, or any location other than the designated front gate **will be immediately ejected** by the venue security team.`,
      },
      {
        slug: "athlete-penalty",
        title: "Penalty for the athlete",
        body: `The athlete associated with the offending parent, relative, or friend will be **prohibited from participating** in the event scheduled for that weekend.`,
      },
      {
        slug: "team-expulsion",
        title: "Team expulsion",
        body: `If a team has parents, relatives, or friends who commit this offense **three times**, the entire team will be immediately expelled from the event and will forfeit all remaining games. **No refunds** will be issued for violations of this policy.`,
      },
      {
        slug: "officials-staff-conduct",
        title: "Conduct toward officials & staff",
        body: `Bad behavior toward referees, scorekeepers, or staff is **not tolerated**. Any violation results in immediate ejection from the venue and may result in a permanent ban from future Super 6 events. **There are no exceptions.**`,
      },
    ],
  },
  {
    id: "uniform",
    number: "02",
    label: "Uniform Compliance",
    title: "Matched, numbered, professional.",
    description:
      "Game officials monitor uniform compliance. Non-compliant teams do not play and are not refunded.",
    icon: "shirt",
    items: [
      {
        slug: "matching-uniforms",
        title: "Matching uniforms",
        body: `All players must wear matching, **unmodified** uniforms. Any player without a matching uniform will not be allowed to play. Super 6 enforces a strict uniform-matching policy and **does not make exceptions**.`,
      },
      {
        slug: "no-tshirts",
        title: "No t-shirts as uniforms",
        body: `T-shirts with markings or designs are **not** considered acceptable uniforms. All players must wear matching, unmodified uniforms to participate.`,
      },
      {
        slug: "pressed-numbers",
        title: "Pressed numbers required",
        body: `All uniforms must have **pressed numbers** — home, away, and reversible sets. This is mandatory for every team.`,
      },
      {
        slug: "no-ink-markings",
        title: "No Sharpie or ink on jerseys",
        body: `**Sharpie**, marker, or **any ink markings** on a **jersey or T-shirt** are **not** permitted. Those athletes **may not play**. **No exceptions**.`,
      },
      {
        slug: "numbers-both-sides",
        title: "Numbers on both sides",
        body: `Uniforms must have numbers on both sides. **No two players on a team may share a number.** Violations may result in removal from the event by the site director or referee.`,
      },
      {
        slug: "coaches-attire",
        title: "Coaches' attire",
        body: `Coaches are required to dress in **collared shirts or team apparel**.`,
      },
      {
        slug: "home-away-colors",
        title: "Home & away colors",
        body: `On the schedule, the home team is listed at the bottom and the away team at the top. **The home team wears white or light-colored uniforms; the away team wears dark-colored uniforms.** Failure to follow this rule may result in a technical foul before the game starts.`,
      },
      {
        slug: "uniform-refunds",
        title: "Refunds",
        body: `Teams that do not meet the uniform standards **will not be permitted to play** and are **not eligible for a refund** under any circumstances.`,
      },
    ],
  },
  {
    id: "team-requirements",
    number: "03",
    label: "Team Requirements",
    title: "What every team must bring.",
    description:
      "Coach roles, insurance, paperwork, balls, and the forfeit fee schedule.",
    icon: "clipboard-list",
    items: [
      {
        slug: "mandatory-coach-roles",
        title: "Mandatory coach roles",
        body: `Each team must field two assistant coaches with assigned roles:

- **Assistant Coach (Home Team)** handles the books.
- **Assistant Coach (Away Team)** handles the clock.

If a team cannot fulfill these roles, **the game will be forfeited.** There are no exceptions.`,
      },
      {
        slug: "insurance",
        title: "Insurance",
        body: `All teams are required to carry **team insurance** to participate in any Super 6 event.`,
      },
      {
        slug: "may-be-asked",
        title: "May be asked to provide",
        body: `Teams may be asked to provide proof of:

- Insurance
- Full matching uniforms (t-shirts not allowed)
- A website or social media page
- Previous tournament participation

Teams unable to meet these requirements will be **disqualified and will not receive a refund**.`,
      },
      {
        slug: "forfeit-policy",
        title: "Forfeit policy",
        body: `Game forfeiting is **not tolerated**. A team that forfeits any game must pay a **$100 fee** before participating in their next game and a **$100 forfeit deposit** before any future Super 6 event. **There are no exceptions.**`,
      },
      {
        slug: "warm-up-balls",
        title: "Warm-up balls",
        body: `Teams must bring their own balls for warm-up. **Super 6 does not provide warm-up balls.** The head referee will select the game ball from the two teams' provided balls.`,
      },
    ],
  },
  {
    id: "game-format",
    number: "04",
    label: "Game Format",
    title: "NFHS rules, Super 6 cadence.",
    description:
      "Game length, timeouts, fouls, mercy, and overtime — the cadence of every Super 6 weekend.",
    icon: "timer",
    items: [
      {
        slug: "governing-rules",
        title: "Governing rules",
        body: `Super 6 games are played according to the rules set by the **National Federation of High School Basketball (NFHS)**, unless specified otherwise below.`,
      },
      {
        slug: "game-length-3-7",
        title: "3rd–7th grade — game length",
        body: `**Two 16-minute halves.** Running clock. The clock stops at the **3-minute mark** before the end of regulation.`,
      },
      {
        slug: "game-length-8-12",
        title: "8th–12th grade — game length",
        body: `**Two 18-minute halves.** Running clock. The clock stops at the **3-minute mark** before the end of regulation.`,
      },
      {
        slug: "timeouts",
        title: "Timeouts",
        body: `**Four 30-second timeouts** per game.`,
      },
      {
        slug: "player-fouls",
        title: "Player fouls",
        body: `A player is disqualified from the game upon their **5th personal foul**.`,
      },
      {
        slug: "bonus-fouls",
        title: "Bonus fouls",
        body: `Each team is allowed **7 bonus fouls per half**. After the 7th, each subsequent foul awards **one free throw** to the opposing team.`,
      },
      {
        slug: "double-bonus",
        title: "Double bonus",
        body: `When a team reaches **10 fouls in a half**, the opposing team is awarded **two free throws** on every subsequent foul.`,
      },
      {
        slug: "mercy-rule",
        title: "Mercy rule",
        body: `If the score difference reaches **20 points**, the game clock runs continuously. If the difference closes to **10 points or fewer**, the clock stops again per standard rules. **If the lead reaches 30 points**, the clock runs straight through the final **3 minutes** with **no** stoppage. **If the trailing team is still down by 30 or more with 5:00 or less remaining in regulation**, the game is an **automatic forfeit** — referees or game officials **stop the game**.`,
      },
      {
        slug: "overtime-first",
        title: "Overtime — first period",
        body: `**One minute (1:00) of play.** Each team is granted **one additional timeout** for the period.`,
      },
      {
        slug: "overtime-second",
        title: "Overtime — second period",
        body: `**Sudden death** — the first team to score wins. Each team is granted **one additional timeout** for the period.`,
      },
    ],
  },
  {
    id: "tiebreakers",
    number: "05",
    label: "Tiebreakers",
    title: "How seeding gets decided.",
    description:
      "Head-to-head precedence, point-differential cap, and the automated coin flip.",
    icon: "trophy",
    items: [
      {
        slug: "head-to-head",
        title: "Head-to-head",
        body: `In a head-to-head matchup, **the result of the head-to-head game takes precedence** over total points.`,
      },
      {
        slug: "point-differential-cap",
        title: "Point differential cap",
        body: `Tiebreakers are calculated using a **15-point differential cap**. If pool points remain tied after the differential calculation, an **automated coin flip** determines pool seeding.`,
      },
      {
        slug: "round-robin-bracket",
        title: "Round-robin bracket seeding",
        body: `If all teams in a round-robin pool have identical records and have all faced each other head-to-head, the **Exposure Events software** will automatically conduct a coin flip to determine bracket seeding.`,
      },
    ],
  },
  {
    id: "player-eligibility",
    number: "06",
    label: "Player Eligibility",
    title: "One team. One division.",
    description:
      "Where players can compete, how grade is verified, and how challenges work.",
    icon: "user-check",
    items: [
      {
        slug: "one-team-per-division",
        title: "One team per division",
        body: `Each player may only compete for **one team in a particular division**. Athletes may always **play up** within the same club. Athletes may **NOT play down**, and may not play across divisions. Violations result in an **automatic game forfeit**.`,
      },
      {
        slug: "grade-verification",
        title: "Grade verification",
        body: `Players must have access to their **school portal** in order to verify grade at any time. The school portal is the only accepted form of grade documentation. Eligibility questions should be addressed with the site director.`,
      },
      {
        slug: "eligibility-challenges",
        title: "Eligibility challenges",
        body: `**Anyone** may file an eligibility challenge. The challenger pays **$100 per athlete challenged** via **Cash App or Venmo only** (not credit card). The **site director** conducts verification **one-on-one with the athlete only** — no coaches, parents, or others. The athlete must **in real time** log in to **their school portal** and show they are in the **correct grade** for that division. The site director may ask **specific questions** (grade, teachers, classes, prior grades) to confirm division fit. **If the athlete fails or cannot complete verification**, **all games** that athlete played or would play **in that division** at the event are **forfeited** (past and future). If the challenge is upheld, the **$100 fee is returned** to the challenger.`,
      },
      {
        slug: "girls-playing-down",
        title: "Girls playing down",
        body: `A girls' team is allowed to play **one level down** in the boys' division (e.g. a 7th-grade girls' team may play in the 6th-grade boys' division). If a girls' team is unable to compete at that level, they will not be permitted to do so in future tournaments. **The determination is at the sole discretion of the Super 6 director.**`,
      },
    ],
  },
  {
    id: "scheduling",
    number: "07",
    label: "Scheduling",
    title: "Game time, tip-off, and check-in.",
    description: "Arrival, check-in, and the readiness deadline.",
    icon: "calendar-clock",
    items: [
      {
        slug: "arrival",
        title: "Arrival",
        body: `Plan to arrive **at least one (1) hour prior** to your scheduled game time.`,
      },
      {
        slug: "mandatory-checkin",
        title: "Mandatory check-in",
        body: `**ALL TEAMS MUST CHECK IN PRIOR TO THEIR 2ND GAME** of the tournament.`,
      },
      {
        slug: "game-start-times",
        title: "Game start times",
        body: `Games begin **five (5) minutes** after the end of the previous game on the same court. Running clocks and forfeits may shift the schedule, which is why **one-hour pre-game arrival is required**.`,
      },
      {
        slug: "readiness-deadline",
        title: "Readiness deadline",
        body: `Teams must be ready to play **no more than five (5) minutes** after the scheduled game time, or **risk forfeit**.`,
      },
    ],
  },
];

/** Total rule count — surfaced in the hero index card. */
export const totalRules = rulesSections.reduce(
  (acc, s) => acc + s.items.length,
  0
);
