/* ─── Impact Strip — Editorial stats bar
   Large serif numbers on cream, hairline dividers, quiet caps labels.
   Anthropic editorial × Players' Tribune masthead. */

const stats = [
  { number: "24K", suffix: "", label: "Athletes Served Annually" },
  { number: "125K", suffix: "", label: "Annual Event Attendees" },
  { number: "2,000", suffix: "+", label: "Annual Teams" },
  { number: "300", suffix: "+", label: "Annual Clubs" },
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
