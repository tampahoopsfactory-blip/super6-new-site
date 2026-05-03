import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Tournament Rules",
  description:
    "Official Super 6 youth basketball tournament rules. Conduct, uniform compliance, game format, player eligibility, and forfeit policy. NFHS standards.",
};

const sections = [
  { id: "conduct", label: "Conduct" },
  { id: "uniform", label: "Uniform Compliance" },
  { id: "team-requirements", label: "Team Requirements" },
  { id: "game-format", label: "Game Format" },
  { id: "tiebreakers", label: "Tiebreakers" },
  { id: "player-eligibility", label: "Player Eligibility" },
  { id: "scheduling", label: "Scheduling" },
];

export default function RulesPage() {
  return (
    <>
      {/* Hero — Editorial Rule Book treatment */}
      <section className="rules-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/refs-crew.jpg"
            alt=""
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center 30%" }}
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">OFFICIAL</span>
            <span className="rules-hero-meta-divider" />
            <span>2026 SEASON · NFHS STANDARD</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">GAME</span>
            <span className="rules-hero-word-rules">
              RULES
              <em className="rules-hero-amp">&</em>
            </span>
            <span className="rules-hero-word-book">RULE BOOK</span>
          </div>

          <h1 className="rules-hero-headline">
            Played by the <em>book.</em>
          </h1>

          <p className="rules-hero-desc">
            Super 6 follows National Federation of High School Basketball
            standards. The rules below govern conduct, uniform compliance,
            game format, eligibility, and scheduling at every Super 6 weekend.
          </p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">07</span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">Sections</span>
              <span className="rules-hero-index-sub">Conduct · Uniform · Format · Tiebreakers · Eligibility · Scheduling</span>
            </div>
          </div>
        </div>
      </section>

      {/* Section nav */}
      <section className="section-warm" style={{ padding: "32px 0", borderBottom: "1px solid var(--hairline-soft)" }}>
        <div className="container-xl">
          <nav
            aria-label="Rules sections"
            style={{
              display: "flex",
              flexWrap: "wrap",
              gap: 8,
              justifyContent: "center",
            }}
          >
            {sections.map((s) => (
              <a
                key={s.id}
                href={`#${s.id}`}
                className="rule-pill"
              >
                {s.label}
              </a>
            ))}
          </nav>
        </div>
      </section>

      {/* CONDUCT */}
      <section id="conduct" className="section">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">01 · Conduct</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                A respectful weekend, <em>or no weekend.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="Admission"
                body={
                  <>
                    <p>
                      Travel sports come with financial pressure on clubs. To
                      keep events affordable and high-quality, everyone except
                      players in uniform and a maximum of two coaches per team
                      is required to pay venue admission.
                    </p>
                    <div className="rules-payment-notice" role="note">
                      <div className="rules-payment-header">
                        <span className="rules-payment-tag">Important</span>
                        <span className="rules-payment-cashout" aria-hidden="true">
                          <span className="rules-payment-cash-word">CASH</span>
                        </span>
                      </div>
                      <p className="rules-payment-headline">
                        We do <em>not</em> accept cash at the gate.
                      </p>
                      <p className="rules-payment-sub">
                        Pay quickly and securely with any of the following —
                        please have your method ready before reaching the front.
                      </p>
                      <ul className="rules-payment-methods">
                        <li className="rules-payment-method">
                          <span className="rules-payment-logo" aria-hidden="true">
                            <svg viewBox="0 0 64 64" width="56" height="56">
                              <rect width="64" height="64" rx="14" fill="#0E0D0B" />
                              <text
                                x="32"
                                y="46"
                                textAnchor="middle"
                                fontFamily="var(--font-display), serif"
                                fontSize="44"
                                fontWeight="700"
                                fill="#fff"
                              >
                                $
                              </text>
                            </svg>
                          </span>
                          <span className="rules-payment-method-name">Cash App</span>
                        </li>

                        <li className="rules-payment-method">
                          <span className="rules-payment-logo" aria-hidden="true">
                            <svg viewBox="0 0 64 64" width="56" height="56">
                              <rect width="64" height="64" rx="14" fill="#0E0D0B" />
                              <text
                                x="32"
                                y="48"
                                textAnchor="middle"
                                fontFamily="var(--font-display), serif"
                                fontSize="42"
                                fontStyle="italic"
                                fontWeight="700"
                                fill="#fff"
                              >
                                v
                              </text>
                            </svg>
                          </span>
                          <span className="rules-payment-method-name">Venmo</span>
                        </li>

                        <li className="rules-payment-method">
                          <span className="rules-payment-logo" aria-hidden="true">
                            <svg viewBox="0 0 64 64" width="56" height="56">
                              <rect width="64" height="64" rx="14" fill="#0E0D0B" />
                              <path
                                d="M33.4 22.4c-1 1.2-2.7 2.2-4.4 2-.2-1.7.6-3.6 1.6-4.7 1.1-1.3 2.9-2.2 4.3-2.3.2 1.8-.5 3.7-1.5 5zm1.5 2.4c-2.5-.1-4.6 1.4-5.8 1.4-1.2 0-3-1.3-5-1.3-2.5 0-4.9 1.5-6.2 3.8-2.7 4.6-.7 11.4 1.9 15.2 1.3 1.9 2.8 4 4.8 3.9 1.9-.1 2.6-1.2 4.9-1.2 2.3 0 3 1.2 5 1.2 2.1 0 3.4-1.9 4.7-3.8 1.5-2.2 2-4.3 2.1-4.4-.1 0-4-1.5-4-6 0-3.7 3.1-5.5 3.2-5.6-1.7-2.5-4.5-2.8-5.5-2.9-2.5-.3-4.6 1.4-5.8 1.4-1.1 0-2.7-1-4.5-1z"
                                fill="#fff"
                              />
                            </svg>
                          </span>
                          <span className="rules-payment-method-name">Apple Pay</span>
                        </li>

                        <li className="rules-payment-method">
                          <span className="rules-payment-logo" aria-hidden="true">
                            <svg viewBox="0 0 64 64" width="56" height="56">
                              <rect width="64" height="64" rx="14" fill="#0E0D0B" />
                              <text
                                x="32"
                                y="46"
                                textAnchor="middle"
                                fontFamily="var(--font-display), serif"
                                fontSize="42"
                                fontWeight="700"
                                fill="#fff"
                              >
                                Z
                              </text>
                            </svg>
                          </span>
                          <span className="rules-payment-method-name">Zelle</span>
                        </li>

                        <li className="rules-payment-method">
                          <span className="rules-payment-logo" aria-hidden="true">
                            <svg viewBox="0 0 64 64" width="56" height="56">
                              <rect width="64" height="64" rx="14" fill="#0E0D0B" />
                              <rect
                                x="14"
                                y="20"
                                width="36"
                                height="24"
                                rx="3"
                                fill="none"
                                stroke="#fff"
                                strokeWidth="2.5"
                              />
                              <line
                                x1="14"
                                y1="27"
                                x2="50"
                                y2="27"
                                stroke="#fff"
                                strokeWidth="2.5"
                              />
                              <rect x="20" y="36" width="9" height="3" fill="#fff" />
                            </svg>
                          </span>
                          <span className="rules-payment-method-name">Credit Cards</span>
                        </li>
                      </ul>
                    </div>
                  </>
                }
              />
              <RuleBlock
                title="Reporting Violations"
                body="To uphold the integrity of our events, please report any violations of the admission or conduct policy to our team."
              />
              <RuleBlock
                title="Badge Exchange & Unauthorized Entry"
                body="Anyone caught exchanging badges or attempting to enter the venue through side doors, exits, or any location other than the designated front gate will be immediately ejected by the venue security team."
              />
              <RuleBlock
                title="Penalty for the Athlete"
                body="The athlete associated with the offending parent, relative, or friend will be prohibited from participating in the event scheduled for that weekend."
              />
              <RuleBlock
                title="Team Expulsion"
                body="If a team has parents, relatives, or friends who commit this offense three times, the entire team will be immediately expelled from the event and will forfeit all remaining games. No refunds will be issued for violations of this policy."
              />
              <RuleBlock
                title="Conduct Toward Officials & Staff"
                body="Bad behavior toward referees, scorekeepers, or staff is not tolerated. Any violation will result in immediate ejection from the venue and may result in a permanent ban from future Super 6 events. There are no exceptions."
              />
            </div>
          </div>
        </div>
      </section>

      {/* UNIFORM */}
      <section id="uniform" className="section section-warm">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">02 · Uniform Compliance</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                Matched, numbered, <em>professional.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <p className="rules-callout">
                <span className="rules-callout-tag">Notice</span>
                Game officials will be monitoring uniform compliance.
              </p>
              <RuleBlock
                title="Matching Uniforms"
                body="All players must wear matching, unmodified uniforms. Any player without a matching uniform will not be allowed to play. The Super 6 enforces a strict uniform-matching policy and does not make exceptions."
              />
              <RuleBlock
                title="No T-Shirts as Uniforms"
                body="T-shirts with markings or designs are not considered acceptable uniforms. All players must wear matching, unmodified uniforms to participate."
              />
              <RuleBlock
                title="Pressed Numbers Required"
                body="All uniforms must have pressed numbers — home, away, and reversible sets. This is mandatory for every team."
              />
              <RuleBlock
                title="Numbers on Both Sides"
                body="Uniforms must have numbers on both sides. No two players on a team may share a number. Violations may result in removal from the event by the site director or referee."
              />
              <RuleBlock
                title="Coaches' Attire"
                body="Coaches are required to dress in collared shirts or team apparel."
              />
              <RuleBlock
                title="Home & Away Colors"
                body="On the schedule, the home team is listed at the bottom and the away team at the top. The home team wears white or light-colored uniforms; the away team wears dark-colored uniforms. A failure to follow this rule may result in a technical foul before the game starts."
              />
              <RuleBlock
                title="Refunds"
                body="Teams that do not meet the uniform standards will not be permitted to play and are not eligible for a refund under any circumstances."
              />
            </div>
          </div>
        </div>
      </section>

      {/* TEAM REQUIREMENTS */}
      <section id="team-requirements" className="section">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">03 · Team Requirements</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                What every team <em>must bring.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="Mandatory Coach Roles"
                body="Each team must field two assistant coaches with assigned roles: Assistant Coach (Home Team) handles the books. Assistant Coach (Away Team) handles the clock. If a team cannot fulfill these roles, the game will be forfeited. There are no exceptions."
              />
              <RuleBlock
                title="Insurance"
                body="All teams are required to carry team insurance to participate in any Super 6 event."
              />
              <RuleBlock
                title="May Be Asked to Provide"
                body={
                  <>
                    Teams may be asked to provide proof of:
                    <ul style={{ marginTop: 12, paddingLeft: 20, lineHeight: 1.8 }}>
                      <li>Insurance</li>
                      <li>Full matching uniforms (t-shirts not allowed)</li>
                      <li>A website or social media page</li>
                      <li>Previous tournament participation</li>
                    </ul>
                    Teams unable to meet these requirements will be disqualified and will not receive a refund.
                  </>
                }
              />
              <RuleBlock
                title="Forfeit Policy"
                body="Game forfeiting is not tolerated. A team that forfeits any game must pay a $100 fee before participating in their next game and a $100 forfeit deposit before any future Super 6 event. There are no exceptions."
              />
              <RuleBlock
                title="Warm-Up Balls"
                body="Teams must bring their own balls for warm-up. The Super 6 does not provide warm-up balls. The head referee will select the game ball from the two teams' provided balls."
              />
            </div>
          </div>
        </div>
      </section>

      {/* GAME FORMAT */}
      <section id="game-format" className="section section-warm">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">04 · Game Format</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                NFHS rules, <em>Super 6 cadence.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="Governing Rules"
                body="Super 6 games are played according to the rules set by the National Federation of High School Basketball (NFHS), unless specified otherwise below."
              />
              <RuleBlock
                title="3rd–7th Grade — Game Length"
                body="Two 16-minute halves. Running clock. The clock stops at the 3-minute mark before the end of regulation."
              />
              <RuleBlock
                title="8th–12th Grade — Game Length"
                body="Two 18-minute halves. Running clock. The clock stops at the 3-minute mark before the end of regulation."
              />
              <RuleBlock
                title="Timeouts"
                body="Four 30-second timeouts per game."
              />
              <RuleBlock
                title="Player Fouls"
                body="A player is disqualified from the game upon their 5th personal foul."
              />
              <RuleBlock
                title="Bonus Fouls"
                body="Each team is allowed 7 bonus fouls per half. After the 7th, each subsequent foul awards one free throw to the opposing team."
              />
              <RuleBlock
                title="Double Bonus"
                body="When a team reaches 10 fouls in a half, the opposing team is awarded two free throws on every subsequent foul."
              />
              <RuleBlock
                title="Mercy Rule"
                body="If the score difference reaches 20 points, the game clock runs continuously. If the difference closes back to 10 points, the clock stops again per standard rules."
              />
              <RuleBlock
                title="Overtime — First Period"
                body="One minute (1:00) of play. Each team is granted one additional timeout for the period."
              />
              <RuleBlock
                title="Overtime — Second Period"
                body="Sudden death — the first team to score wins. Each team is granted one additional timeout for the period."
              />
            </div>
          </div>
        </div>
      </section>

      {/* TIEBREAKERS */}
      <section id="tiebreakers" className="section">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">05 · Tiebreakers</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                How seeding gets <em>decided.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="Head-to-Head"
                body="In a head-to-head matchup, the result of the head-to-head game takes precedence over total points."
              />
              <RuleBlock
                title="Point Differential Cap"
                body="Tiebreakers are calculated using a 15-point differential cap. If pool points remain tied after the differential calculation, an automated coin flip determines pool seeding."
              />
              <RuleBlock
                title="Round-Robin Bracket Seeding"
                body="If all teams in a round-robin pool have identical records and have all faced each other head-to-head, the Exposure Events software will automatically conduct a coin flip to determine bracket seeding."
              />
            </div>
          </div>
        </div>
      </section>

      {/* PLAYER ELIGIBILITY */}
      <section id="player-eligibility" className="section section-warm">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">06 · Player Eligibility</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                One team. <em>One division.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="One Team Per Division"
                body="Each player may only compete for one team in a particular division. Athletes may always play UP within the same club. Athletes may NOT play down, and may not play across divisions. Violations result in an automatic game forfeit."
              />
              <RuleBlock
                title="Grade Verification"
                body="Players must have access to their school portal in order to verify grade at any time. The school portal is the only accepted form of grade documentation. Eligibility questions should be addressed with the site director."
              />
              <RuleBlock
                title="Eligibility Challenges"
                body="Eligibility challenges are accepted at a fee of $100 per athlete challenged. The site coordinator works with the head coach to review the proof of eligibility. If proper documentation is not provided, the player will be removed from the tournament. If a game has already been played and the player is found ineligible, the team forfeits that game. If the challenge is upheld, the $100 fee is returned to the challenger."
              />
              <RuleBlock
                title="Girls Playing Down"
                body="A girls' team is allowed to play one level down in the boys' division (e.g. a 7th-grade girls' team may play in the 6th-grade boys' division). If a girls' team is unable to compete at that level, they will not be permitted to do so in future tournaments. The determination is at the sole discretion of the Super 6 director."
              />
            </div>
          </div>
        </div>
      </section>

      {/* SCHEDULING */}
      <section id="scheduling" className="section">
        <div className="container-xl">
          <div className="rules-layout">
            <aside className="rules-side">
              <p className="section-label">07 · Scheduling</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                Game time, <em>tip-off, and check-in.</em>
              </h2>
            </aside>
            <div className="rules-body">
              <RuleBlock
                title="Arrival"
                body="Plan to arrive at least one (1) hour prior to your scheduled game time."
              />
              <RuleBlock
                title="Mandatory Check-In"
                body="ALL TEAMS MUST CHECK IN PRIOR TO THEIR 2ND GAME of the tournament."
              />
              <RuleBlock
                title="Game Start Times"
                body="Games begin five (5) minutes after the end of the previous game on the same court. Running clocks and forfeits may shift the schedule, which is why one-hour pre-game arrival is required."
              />
              <RuleBlock
                title="Readiness Deadline"
                body="Teams must be ready to play no more than five (5) minutes after the scheduled game time, or risk forfeit."
              />
            </div>
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="cta-section" aria-label="Register for next event">
        <div className="container-xl">
          <h2 className="cta-heading">
            Read the rules. <em>Bring the team.</em>
          </h2>
          <p className="cta-sub">
            Now that you know how the weekend works, lock in your spot.
          </p>
          <div style={{ display: "flex", gap: 14, justifyContent: "center", flexWrap: "wrap" }}>
            <Link href="/register" className="btn btn-orange">Register your team</Link>
            <Link href="/contact" className="btn btn-outline-light">Question? Contact us</Link>
          </div>
        </div>
      </section>
    </>
  );
}

/* ─── Reusable rule block — title + paragraph or rich content ─── */
function RuleBlock({
  title,
  body,
}: {
  title: string;
  body: React.ReactNode;
}) {
  return (
    <div className="rule-block">
      <h3 className="rule-block-title">{title}</h3>
      <div className="rule-block-body">{body}</div>
    </div>
  );
}
