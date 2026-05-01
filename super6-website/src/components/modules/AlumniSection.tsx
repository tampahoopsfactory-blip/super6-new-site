/* ─── Alumni College Placements — Ivy League / high-academic outcomes.
   Source: V2 Investor Deck. Verified placements only. */

const featured = [
  {
    name: "Tim Keeley II",
    college: "Brown · UCSF Medical",
    body: "Recruited by Yale and MIT. Graduated Brown University with honors in Biology — 3.9 GPA. National Honor Society. Research internships at Brown, University Orthopedics, and Moffitt Cancer Center. Currently a 4th-year medical student at the University of California, San Francisco (UCSF), pursuing Sports Orthopedic Surgery.",
  },
  {
    name: "Zion Carter",
    college: "Dartmouth College",
    body: "Enrolled at 14. Recruited for basketball by Princeton, MIT, Hobart, Claremont. Recruited for football by Columbia, Brown, Princeton, Dartmouth. National Honor Society. Currently at Dartmouth playing football. Apple finance intern (Summer 2022).",
  },
  {
    name: "Varun Ajjarapu",
    college: "Penn State (via U.S. Coast Guard Academy)",
    body: "Enrolled at 12. Recruited by MIT, University of Chicago, Harvard, Dartmouth, Emory, NYU, Johns Hopkins, U.S. Coast Guard Academy. National Honor Society. Goal: orthopedic surgeon in the NBA.",
  },
];

const colleges = [
  "Brown University",
  "Dartmouth College",
  "MIT",
  "University of Chicago",
  "Harvard University",
  "Princeton University",
  "Columbia University",
  "Notre Dame",
  "Emory University",
  "NYU",
  "Johns Hopkins",
  "U.S. Coast Guard Academy",
  "University of Michigan",
  "Penn State",
  "UCSF Medical School",
];

export default function AlumniSection() {
  return (
    <section
      className="alumni-section section"
      aria-labelledby="alumni-heading"
    >
      <div className="container-xl">
        <div className="alumni-intro">
          <p className="section-label">College Pipeline</p>
          <h2 id="alumni-heading" className="section-heading">
            Where our alumni <em>land.</em>
          </h2>
          <p className="section-desc">
            Super 6 athletes are recruited by — and enroll at — the most
            selective academic institutions in the country. Three featured
            stories below; the placement list runs deeper.
          </p>
        </div>

        <div className="alumni-grid">
          {featured.map((a) => (
            <article key={a.name} className="alumni-card">
              <p className="alumni-college">{a.college}</p>
              <h3 className="alumni-name">{a.name}</h3>
              <p className="alumni-body">{a.body}</p>
            </article>
          ))}
        </div>

        <div className="alumni-college-list">
          <p className="alumni-college-list-label">
            Alumni placements include
          </p>
          <ul>
            {colleges.map((c) => (
              <li key={c}>{c}</li>
            ))}
          </ul>
        </div>
      </div>
    </section>
  );
}
