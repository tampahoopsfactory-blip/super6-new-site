"use client";

import { useState } from "react";
import Image from "next/image";
import { siteConfig } from "@/data/site";

export default function ContactPage() {
  const [formState, setFormState] = useState({
    name: "",
    email: "",
    phone: "",
    subject: "general",
    message: "",
  });
  const [submitted, setSubmitted] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitted(true);
  };

  return (
    <>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/12-coach-intensity.jpg"
          alt=""
          fill
          priority
          sizes="100vw"
          quality={92}
          aria-hidden="true"
        />
        <div className="container-xl">
          <p className="editorial-eyebrow" style={{ color: "var(--cream)", opacity: 0.85 }}>
            Get in Touch
          </p>
          <h1>
            Have a question? <em>We&rsquo;re listening.</em>
          </h1>
          <p>
            Registration, venue partnerships, sponsorships, press &mdash;
            we&rsquo;ll get back to you within one business day.
          </p>
        </div>
      </section>

      {/* Contact + form */}
      <section className="section">
        <div className="container-xl">
          <div className="contact-grid">
            {/* Contact info */}
            <div>
              <p className="section-label">Reach Us</p>
              <h2
                className="section-heading"
                style={{ fontSize: "clamp(28px, 3vw, 40px)", marginBottom: 32 }}
              >
                Direct lines.
              </h2>

              <div style={{ display: "flex", flexDirection: "column", gap: 28 }}>
                <div>
                  <p
                    style={{
                      fontFamily: "var(--font-body)",
                      fontSize: 12,
                      fontWeight: 500,
                      letterSpacing: "0.14em",
                      textTransform: "uppercase",
                      color: "var(--orange)",
                      marginBottom: 8,
                    }}
                  >
                    Headquarters
                  </p>
                  <p style={{ fontSize: 16, color: "var(--ink-soft)", lineHeight: 1.6 }}>
                    {siteConfig.address}
                  </p>
                </div>

                <div>
                  <p
                    style={{
                      fontFamily: "var(--font-body)",
                      fontSize: 12,
                      fontWeight: 500,
                      letterSpacing: "0.14em",
                      textTransform: "uppercase",
                      color: "var(--orange)",
                      marginBottom: 8,
                    }}
                  >
                    Phone
                  </p>
                  <a
                    href={`tel:${siteConfig.phone}`}
                    style={{
                      fontFamily: "var(--font-display)",
                      fontSize: 28,
                      color: "var(--ink)",
                      letterSpacing: "-0.014em",
                      textDecoration: "none",
                    }}
                  >
                    {siteConfig.phone}
                  </a>
                </div>

                <div>
                  <p
                    style={{
                      fontFamily: "var(--font-body)",
                      fontSize: 12,
                      fontWeight: 500,
                      letterSpacing: "0.14em",
                      textTransform: "uppercase",
                      color: "var(--orange)",
                      marginBottom: 8,
                    }}
                  >
                    Email
                  </p>
                  <a
                    href={`mailto:${siteConfig.email}`}
                    style={{
                      fontFamily: "var(--font-display)",
                      fontSize: 24,
                      color: "var(--ink)",
                      letterSpacing: "-0.012em",
                      textDecoration: "none",
                      borderBottom: "1px solid var(--hairline)",
                    }}
                  >
                    {siteConfig.email}
                  </a>
                </div>

                <div>
                  <p
                    style={{
                      fontFamily: "var(--font-body)",
                      fontSize: 12,
                      fontWeight: 500,
                      letterSpacing: "0.14em",
                      textTransform: "uppercase",
                      color: "var(--orange)",
                      marginBottom: 8,
                    }}
                  >
                    Tournament Weekends
                  </p>
                  <p style={{ fontSize: 15, color: "var(--slate)", lineHeight: 1.6 }}>
                    Schedule + venue map sent to all registered teams 7 days before tip-off.
                  </p>
                </div>
              </div>
            </div>

            {/* Form */}
            <div>
              <div
                style={{
                  background: "var(--paper)",
                  border: "1px solid var(--hairline-soft)",
                  borderRadius: "var(--radius-lg)",
                  padding: 36,
                }}
              >
                <p className="section-label" style={{ marginBottom: 14 }}>Send Us a Note</p>
                <h3
                  style={{
                    fontFamily: "var(--font-display)",
                    fontSize: 28,
                    fontWeight: 400,
                    letterSpacing: "-0.018em",
                    color: "var(--ink)",
                    marginBottom: 24,
                  }}
                >
                  We&rsquo;ll get right back.
                </h3>

                {submitted ? (
                  <div style={{ padding: "32px 0", textAlign: "center" }}>
                    <p
                      style={{
                        fontFamily: "var(--font-display)",
                        fontSize: 22,
                        color: "var(--orange)",
                        marginBottom: 8,
                      }}
                    >
                      Message sent.
                    </p>
                    <p style={{ fontSize: 15, color: "var(--slate)" }}>
                      We&rsquo;ll be in touch within one business day.
                    </p>
                  </div>
                ) : (
                  <form onSubmit={handleSubmit} className="contact-form">
                    <div className="form-row">
                      <div className="form-field">
                        <label htmlFor="name">Full Name *</label>
                        <input
                          type="text"
                          id="name"
                          required
                          value={formState.name}
                          onChange={(e) =>
                            setFormState({ ...formState, name: e.target.value })
                          }
                          placeholder="Your name"
                        />
                      </div>
                      <div className="form-field">
                        <label htmlFor="email">Email *</label>
                        <input
                          type="email"
                          id="email"
                          required
                          value={formState.email}
                          onChange={(e) =>
                            setFormState({ ...formState, email: e.target.value })
                          }
                          placeholder="your@email.com"
                        />
                      </div>
                    </div>

                    <div className="form-row">
                      <div className="form-field">
                        <label htmlFor="phone">Phone</label>
                        <input
                          type="tel"
                          id="phone"
                          value={formState.phone}
                          onChange={(e) =>
                            setFormState({ ...formState, phone: e.target.value })
                          }
                          placeholder="(555) 123-4567"
                        />
                      </div>
                      <div className="form-field">
                        <label htmlFor="subject">Subject</label>
                        <select
                          id="subject"
                          value={formState.subject}
                          onChange={(e) =>
                            setFormState({ ...formState, subject: e.target.value })
                          }
                        >
                          <option value="general">General Inquiry</option>
                          <option value="registration">Team Registration</option>
                          <option value="venue">Venue Partnership</option>
                          <option value="sponsorship">Sponsorship</option>
                          <option value="media">Media / Press</option>
                        </select>
                      </div>
                    </div>

                    <div className="form-field">
                      <label htmlFor="message">Message *</label>
                      <textarea
                        id="message"
                        required
                        rows={5}
                        value={formState.message}
                        onChange={(e) =>
                          setFormState({ ...formState, message: e.target.value })
                        }
                        placeholder="Tell us how we can help..."
                      />
                    </div>

                    <button type="submit" className="btn btn-orange" style={{ width: "100%" }}>
                      Send Message
                    </button>
                  </form>
                )}
              </div>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
