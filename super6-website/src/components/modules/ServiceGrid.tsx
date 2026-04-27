/* ─── Impact Strip — Editorial stats bar
   Large serif numbers on cream, hairline dividers, quiet caps labels.
   Anthropic editorial × Players' Tribune masthead. */

const stats = [
  { number: "12", suffix: "+", label: "Years Running" },
  { number: "400", suffix: "+", label: "Tournaments Hosted" },
  { number: "1,000", suffix: "+", label: "Teams Served" },
  { number: "5", suffix: "", label: "Cities, Two States" },
];

export default function ImpactStrip() {
  return (
    <section className="impact-strip" aria-label="By the numbers">
      <div className="container-xl">
        <div className="impact-grid">
          {stats.map((stat) => (
            <div key={stat.label} className="impact-item">
              <div className="impact-number">
                {stat.number}
                <span className="accent">{stat.suffix}</span>
              </div>
              <div className="impact-label">{stat.label}</div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
