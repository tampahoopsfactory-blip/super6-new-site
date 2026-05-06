/* ─── Programs Section — six core program lines.
   Editorial card grid on warm cream. Source: V2 Investor Deck. */

const programs = [
  {
    name: "Athletic Event Operations",
    status: "Core · Active",
    body: "Weekly sold-out youth basketball tournaments. Most affordable in Florida — $99 per team. NFHS-certified officials. Media coverage. Best-in-class production.",
  },
  {
    name: "BooksFirst Academic Program",
    status: "Active",
    body: "College counseling, SAT/ACT prep, all-subject tutoring, career-development workshops, internships, and networking — through partnerships with Ivy League, HBCU, and traditional colleges.",
  },
  {
    name: "Super6 Series LLC TV",
    status: "Active · HBCU League Pass",
    body: "Live event streaming, game recordings, commentary, and pre/post-game interviews. Carried on HBCU League Pass. Building toward the ESPN of youth sports.",
  },
];

export default function ProgramsSection() {
  return (
    <section
      className="programs-section section section-warm"
      aria-labelledby="programs-heading"
    >
      <div className="container-xl">
        <div className="programs-intro">
          <p className="section-label">Programs &amp; Services</p>
          <h2 id="programs-heading" className="section-heading">
            More than a tournament. <em>A pipeline.</em>
          </h2>
          <p className="section-desc">
            Six active program lines built to serve the same athlete from
            ages 3 to 18 — on the court, in the classroom, and into college.
          </p>
        </div>

        <div className="programs-grid">
          {programs.map((p) => (
            <article key={p.name} className="program-card">
              <div className="program-card-header">
                <h3 className="program-name">{p.name}</h3>
                <span className="program-status">{p.status}</span>
              </div>
              <p className="program-body">{p.body}</p>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}
