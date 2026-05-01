/* ─── Testimonials — verified quotes from coaches, parents, and trainers.
   Source: V2 Investor Deck. */

const testimonials = [
  {
    quote:
      "SportsSpace has been beautiful and exactly what Tampa has needed. The flexibility is amazing. SportsSpace is actually the best gym in Tampa when you consider affordability, comfort, and being able to perform your job at a high level.",
    name: "Brian Turner",
    role: "NYC Gritty Basketball — Coach & Trainer",
  },
  {
    quote:
      "The Super 6 event has been great. Well organized, intense competition. Super 6 brings talent from all over the state that really pushes the kids to play harder. It's a great place to be.",
    name: "Tampa Bayhawks",
    role: "Parents of Bayhawks Athletes",
  },
  {
    quote:
      "The quality of Super 6's $79 tournaments is just the same as significantly more expensive tournaments. To only pay $79 per tournament and enjoy the beaches of Florida and the amusement parks — you cannot beat that deal.",
    name: "James Kuan",
    role: "Acaciawood Basketball, California",
  },
  {
    quote:
      "SportsSpace has been exceptional. The flexibility to get in the gym whenever you want opens up a lot of doors and gives me the ability to grow my business and add clients I might not be able to add.",
    name: "Jordan Fair",
    role: "Progression Daily — Trainer",
  },
];

export default function TestimonialsSection() {
  return (
    <section
      className="testimonials-section section section-warm"
      aria-labelledby="testimonials-heading"
    >
      <div className="container-xl">
        <div className="testimonials-intro">
          <p className="section-label">In Their Words</p>
          <h2 id="testimonials-heading" className="section-heading">
            What coaches, parents, and trainers <em>actually say.</em>
          </h2>
        </div>

        <div className="testimonials-grid">
          {testimonials.map((t) => (
            <figure key={t.name} className="testimonial-card">
              <blockquote className="testimonial-quote">
                &ldquo;{t.quote}&rdquo;
              </blockquote>
              <figcaption className="testimonial-attribution">
                <span className="testimonial-name">{t.name}</span>
                <span className="testimonial-role">{t.role}</span>
              </figcaption>
            </figure>
          ))}
        </div>
      </div>
    </section>
  );
}
