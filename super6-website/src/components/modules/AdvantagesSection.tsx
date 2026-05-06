/* ─── Super6 Series LLC Advantages — milestone stats + competitive edge.
   Editorial dark/cream variant. Source: V2 Final Investor Deck
   (April 30, 2026). All figures verified. */

const advantages = [
  {
    eyebrow: "Cost",
    headline: "$99 vs. $375.",
    body: "We run the lowest-cost youth basketball tournaments in Florida — $99 per team, when the industry standard is $375 or more. Same officiating standard. Same production. Better access.",
  },
  {
    eyebrow: "Streaming",
    headline: "Live on HBCU League Pass.",
    body: "Super6 Series LLC TV streams live tournament play on HBCU League Pass with game recordings, commentary, and pre/post-game interviews. Building the ESPN of youth sports.",
  },
  {
    eyebrow: "Worldwide Reach",
    headline: "Hong Kong to Puerto Rico.",
    body: "Teams travel from Hong Kong, Puerto Rico, and across the continental US to play Super6 Series LLC weekends. Weekly sold-out events.",
  },
  {
    eyebrow: "Corporate Partnership",
    headline: "Wells Fargo financial literacy.",
    body: "Wells Fargo Bank delivers financial literacy seminars to Super6 Series LLC student-athletes — money skills built alongside basketball IQ.",
  },
];

export default function AdvantagesSection() {
  return (
    <section
      className="advantages-section"
      aria-labelledby="advantages-heading"
    >
      <div className="container-xl">
        <div className="advantages-intro">
          <p className="section-label" style={{ color: "var(--orange)" }}>
            The Super6 Series LLC Advantage
          </p>
          <h2 id="advantages-heading" className="section-heading">
            Built different. <em>Priced fair.</em>
          </h2>
          <p className="section-desc">
            Twelve years of iterating on what a youth tournament should
            actually deliver — for athletes, for parents, and for the next
            stage of their career.
          </p>
        </div>

        <div className="advantages-grid">
          {advantages.map((a) => (
            <article key={a.headline} className="advantage-card">
              <p className="advantage-eyebrow">{a.eyebrow}</p>
              <h3 className="advantage-headline" dangerouslySetInnerHTML={{ __html: a.headline }} />
              <p className="advantage-body">{a.body}</p>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}
