import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "FAQ | Super6 Basketball",
  description:
    "Answers to the most common questions about Super6 tournaments — registration, divisions, pricing, venues, and more.",
};

const faqs = [
  {
    category: "Registration",
    items: [
      {
        q: "How do I register my team?",
        a: "Click Register on any event listing or visit thesuper6.com/register. You'll choose your event, select your division, and complete payment online. Registration confirmation is sent immediately.",
      },
      {
        q: "Can I register for multiple events at once?",
        a: "Yes. The $899 season pass covers all 10 events on the calendar. If you prefer single events, register for each individually at $99 per event. Multi-team discounts are available — contact us directly.",
      },
      {
        q: "What is the registration deadline?",
        a: "We recommend registering at least two weeks before each event. Spots fill quickly, especially for competitive divisions. Late entries may be accepted based on availability — contact us to check.",
      },
      {
        q: "Can I cancel or transfer my registration?",
        a: "Transfers to another event are allowed up to 7 days before the event date, subject to availability. Refunds are not issued within 7 days of an event. Contact us as early as possible if your plans change.",
      },
    ],
  },
  {
    category: "Divisions & Format",
    items: [
      {
        q: "What age divisions do you offer?",
        a: "Every Super6 event offers three divisions: 3rd–5th grade, 6th–8th grade, and 9th–12th grade. We bracket by both age and skill level within each division to ensure competitive matchups.",
      },
      {
        q: "How many games does each team play?",
        a: "Every team is guaranteed a minimum of three games per event. Teams that advance through the bracket play additional games. No team goes home after one loss.",
      },
      {
        q: "How are brackets determined?",
        a: "Brackets are seeded based on registration order and, where applicable, prior Super6 performance. We adjust brackets to prevent rematches and ensure skill-appropriate competition. Brackets are posted 48 hours before each event.",
      },
      {
        q: "What if my team has players spanning two grade divisions?",
        a: "Teams register for a single division based on the majority of their players or the oldest player on the roster. Contact us before registering if your roster is split — we will place you appropriately.",
      },
    ],
  },
  {
    category: "Venues & Logistics",
    items: [
      {
        q: "Where are Super6 events held?",
        a: "The 2026 season runs across Orlando, Tampa, Clearwater, West Palm Beach (all Florida), and Atlanta (Georgia). Specific venue addresses are posted with each event listing once confirmed.",
      },
      {
        q: "What time do events start?",
        a: "Events run Saturday–Sunday. Tip-off times typically begin between 8:00 AM and 9:00 AM. Your specific game schedule is released 48 hours before the event via email and the Super6 website.",
      },
      {
        q: "Is there parking at the venues?",
        a: "All venues include on-site or adjacent parking. Parking details are included in your pre-event email. Arrive 30 minutes before your first game to allow for check-in.",
      },
      {
        q: "Are spectators allowed?",
        a: "Yes. Spectators are welcome at all Super6 events. Some venues charge a small admission fee at the door. Specific venue policies are noted in your pre-event communication.",
      },
    ],
  },
  {
    category: "Pricing",
    items: [
      {
        q: "What does entry include?",
        a: "Entry covers all your team's games for the event, court time, scorekeeping, and bracket management. Awards are provided at select events for division finalists and champions.",
      },
      {
        q: "Are there discounts for multiple teams from the same program?",
        a: "Yes. Programs registering three or more teams receive discounted rates. Contact us directly at thesuper6.com/contact to arrange multi-team pricing before registering.",
      },
      {
        q: "Is the season pass per team or per program?",
        a: "The $899 season pass is per team. Programs with multiple teams can purchase individual passes per team. Multi-team season pass discounts are available — contact us.",
      },
    ],
  },
];

export default function FAQPage() {
  return (
    <main>
      {/* Hero */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "7rem 0 5rem",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "760px" }}>
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)" }}
          >
            FAQ
          </p>
          <h1 style={{ color: "var(--cream)" }}>
            Questions,
            <br />
            <em>answered.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.1rem",
              maxWidth: "520px",
              marginTop: "1.25rem",
            }}
          >
            Everything you need to know about registering, competing, and
            getting the most out of Super6.
          </p>
        </div>
      </section>

      {/* FAQ sections */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "760px" }}>
          {faqs.map((section, si) => (
            <div
              key={si}
              style={{ marginBottom: si < faqs.length - 1 ? "4rem" : 0 }}
            >
              <p
                className="editorial-eyebrow"
                style={{ color: "var(--s6-orange)", marginBottom: "1.5rem" }}
              >
                {section.category}
              </p>
              <div style={{ display: "flex", flexDirection: "column", gap: 0 }}>
                {section.items.map((item, ii) => (
                  <div
                    key={ii}
                    style={{
                      padding: "1.75rem 0",
                      borderBottom: "1px solid var(--hairline-soft)",
                    }}
                  >
                    <h3
                      style={{
                        fontSize: "1rem",
                        fontWeight: 700,
                        color: "var(--s6-black)",
                        marginBottom: "0.65rem",
                      }}
                    >
                      {item.q}
                    </h3>
                    <p
                      style={{
                        fontSize: "0.95rem",
                        color: "var(--text-muted)",
                        lineHeight: 1.7,
                        margin: 0,
                      }}
                    >
                      {item.a}
                    </p>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Still have questions */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "520px" }}>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Still have questions?
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            We respond fast. Reach out and we will get you sorted before your
            next event.
          </p>
          <Link
            href="/contact"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Contact Us
          </Link>
        </div>
      </section>
    </main>
  );
}
