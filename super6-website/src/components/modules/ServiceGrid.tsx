/* ─── Impact Strip — Nike: bold stats on dark.
   Monochromatic authority strip between hero and content. ─── */

const stats = [
  { number: "400+", label: "Tournaments Run" },
  { number: "12+", label: "Years Running" },
  { number: "5", label: "Markets" },
  { number: "1,000+", label: "Teams Served" },
];

export default function ImpactStrip() {
  return (
    <section className="impact-strip" aria-label="Impact numbers">
      <div className="container-xl">
        <div className="impact-grid">
          {stats.map((stat) => (
            <div key={stat.label} className="impact-item">
              <div className="impact-number">{stat.number}</div>
              <div className="impact-label">{stat.label}</div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
