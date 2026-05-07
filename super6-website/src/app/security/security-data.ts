/* ─── /security source of truth — Super6 event security standard.
   Edit body content here. Markdown supported (bold, italics, links, lists).
   Slugs are stable — used for /security#slug deep-links.

   Mirrors the structure of src/app/rules/rules-data.ts so the editorial
   pattern (sticky rail, scroll-spy, single-open accordion, deep-link
   expand) can be reused verbatim. */

export type SecurityItem = {
  title: string;
  body: string;
  slug: string;
  pending?: boolean;
};

export type SecurityIcon =
  | "shield-check"
  | "scan-search"
  | "users"
  | "alert-triangle"
  | "badge-check";

export type SecuritySection = {
  id: string;
  number: string;
  label: string;
  title: string;
  description?: string;
  icon: SecurityIcon;
  items: SecurityItem[];
};

export const securityIntro = {
  eyebrow: "Event Security",
  title: "The standard your families expect.",
  body:
    "Licensed personnel at entries, on the floor, and in parking areas — every Super6 weekend. Every bag checked, every entrance covered, every minute on the clock earned. Our staff keep events safe, calm, and orderly while hundreds of families, athletes, and staff share one building.",
};

export const securitySections: SecuritySection[] = [
  {
    id: "personnel",
    number: "01",
    label: "Personnel",
    title: "Trained, licensed, on every gate.",
    description:
      "The crew on the floor is contracted, certified, and the same standard at every venue.",
    icon: "shield-check",
    items: [
      {
        slug: "licensed-officers",
        title: "Licensed officers at every gate",
        body: `Licensed officers are stationed at **every gate, on the floor, and in the parking lot** — the same crew that reads the room before it turns. They diffuse tension, redirect energy, and step in long before anything flares up.`,
      },
      {
        slug: "contracted-coverage",
        title: "Contracted, professional coverage",
        body: `Super6 contracts professional event security for every weekend — not volunteers, not staff playing double duty. The standard is the same in Tampa, Orlando, and Atlanta: **licensed, trained, accountable**.`,
      },
      {
        slug: "uniformed-identifiable",
        title: "Uniformed and identifiable",
        body: `All security personnel are in clearly marked uniforms with visible credentials. If you need help during an event, look for the security shirt — they are there to assist families, not just enforce policy.`,
      },
    ],
  },
  {
    id: "entrance",
    number: "02",
    label: "Entrance Protocol",
    title: "Every bag, every entrance, every time.",
    description:
      "What happens at the front door before anyone reaches the court.",
    icon: "scan-search",
    items: [
      {
        slug: "every-bag-inspected",
        title: "Every bag inspected",
        body: `**Bag checks at every gate, every entry.** We screen for weapons, contraband, and anything that doesn't belong inside a youth event. Have your bag open and ready when you reach the front of the line — it keeps the line moving for the families behind you.`,
      },
      {
        slug: "weapons-screening",
        title: "Weapons & contraband screening",
        body: `Weapons of any kind, alcohol, illegal substances, and outside food/drink are **not permitted** in the venue. Items that don't pass screening must return to the vehicle before entry — no exceptions.`,
      },
      {
        slug: "designated-front-gate",
        title: "Designated front gate only",
        body: `**All entry must be through the designated front gate.** Side doors, exits, and any other location are not permitted entry points. Anyone caught attempting to enter through an unauthorized location will be **immediately ejected** by venue security.`,
      },
      {
        slug: "badge-exchange",
        title: "Badge exchange policy",
        body: `Anyone caught exchanging admission badges or wristbands **will be immediately ejected**. Badges are non-transferable.`,
      },
    ],
  },
  {
    id: "floor-crowd",
    number: "03",
    label: "Floor & Crowd",
    title: "On the floor, in the lot, in the room.",
    description:
      "Active monitoring beyond the gate — courts, parking, and the spaces between.",
    icon: "users",
    items: [
      {
        slug: "active-floor-monitoring",
        title: "Active floor monitoring",
        body: `Security maintains active presence on the courts and in spectator areas throughout the day. The crew watches for tension before it becomes a problem and intervenes early — quietly, calmly, and without disrupting play.`,
      },
      {
        slug: "parking-coverage",
        title: "Parking lot coverage",
        body: `Coverage extends to **parking areas** — not just inside the building. Officers are visible in the lots throughout the event so families feel safe arriving, leaving, and accessing their vehicles between games.`,
      },
      {
        slug: "de-escalation",
        title: "De-escalation first",
        body: `Our standard is de-escalation. The crew reads the room before it turns, diffuses tension, and redirects energy. Removal is the last step, not the first — but when it's needed, it happens immediately.`,
      },
    ],
  },
  {
    id: "enforcement",
    number: "04",
    label: "Enforcement",
    title: "Zero-tolerance, no exceptions.",
    description:
      "What gets you removed, what gets your team penalized, and what gets you banned.",
    icon: "alert-triangle",
    items: [
      {
        slug: "zero-tolerance",
        title: "Zero-tolerance enforcement",
        body: `Disrespect, threats, or unauthorized entry meet **immediate removal**. Every family feels the difference the second they walk in — the standard is consistent, predictable, and applied to everyone.`,
      },
      {
        slug: "officials-staff-respect",
        title: "Respect for officials & staff",
        body: `Bad behavior toward referees, scorekeepers, or staff is **not tolerated**. Any violation results in immediate ejection from the venue and may result in a permanent ban from future Super6 events. **There are no exceptions.**`,
      },
      {
        slug: "athlete-penalty",
        title: "Penalty for the athlete",
        body: `The athlete associated with the offending parent, relative, or friend will be **prohibited from participating** in the event scheduled for that weekend. Conduct violations cascade from spectator to athlete to team — read the [tournament rules](/rules#conduct) for the full chain.`,
      },
      {
        slug: "team-expulsion",
        title: "Team expulsion",
        body: `If a team has parents, relatives, or friends who commit gate or conduct offenses **three times**, the entire team will be **immediately expelled** from the event and will forfeit all remaining games. **No refunds** will be issued for violations of this policy.`,
      },
      {
        slug: "reporting-violations",
        title: "Reporting violations",
        body: `To uphold the integrity of our events, please report any violations to our team via the [Contact page](/contact). Reports are reviewed same-day during the week.`,
      },
    ],
  },
];

export const totalSecurityItems = securitySections.reduce(
  (acc, s) => acc + s.items.length,
  0
);
