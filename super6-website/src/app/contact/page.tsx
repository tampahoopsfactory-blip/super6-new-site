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
      {/* Hero — Editorial Direct Line treatment */}
      <section className="contact-hero">
        <div className="contact-hero-photo">
          <Image
            src="/media/uploads/team-staff.jpg"
            alt=""
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center 35%" }}
          />
        </div>

        <div className="contact-hero-panel">
          <div className="contact-hero-meta">
            <span className="contact-hero-meta-tag">Direct Line</span>
            <span className="contact-hero-meta-divider" />
            <span>RESPONSE WITHIN 1 BUSINESS DAY</span>
          </div>

          <div className="contact-hero-wordmark" aria-hidden="true">
            <span className="contact-hero-word-primary">REACH</span>
            <span className="contact-hero-word-accent">OUT.</span>
            <span className="contact-hero-word-tag">Open Line</span>
          </div>

          <h1 className="contact-hero-headline">
            Have a question? <em>We&rsquo;re listening.</em>
          </h1>

          <p className="contact-hero-desc">
            Registration, venue partnerships, sponsorships, press — write us.
            A real human on the Super 6 team will write you back within one
            business day.
          </p>

          <div className="contact-hero-channels">
            <a
              className="contact-hero-channel"
              href={`mailto:${siteConfig.email}`}
            >
              <span className="contact-hero-channel-label">Email</span>
              <span className="contact-hero-channel-value">{siteConfig.email}</span>
            </a>
            <a
              className="contact-hero-channel"
              href={`tel:${siteConfig.phone}`}
            >
              <span className="contact-hero-channel-label">Phone</span>
              <span className="contact-hero-channel-value">{siteConfig.phone}</span>
            </a>
          </div>
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
