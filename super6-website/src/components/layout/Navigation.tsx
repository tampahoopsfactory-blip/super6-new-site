"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import Image from "next/image";
import { usePathname } from "next/navigation";
import { Menu, X } from "lucide-react";
import { announcements } from "@/data/site";
import { REGISTER_LINK_PROPS } from "@/lib/links";

const navLinks = [
  { label: "Home", href: "/" },
  { label: "Our Mission", href: "/about" },
  { label: "Tournaments", href: "/register" },
  { label: "Gallery", href: "/gallery" },
  { label: "Coaches", href: "/coaches" },
  { label: "Hiring Officials", href: "/officials" },
  { label: "AI Events", href: "/ai-events" },
  { label: "Rules", href: "/rules" },
  { label: "FAQ", href: "/faq" },
  { label: "Contact", href: "/contact" },
];

export default function Navigation() {
  const pathname = usePathname();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const [announcementIdx, setAnnouncementIdx] = useState(0);

  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 10);
    window.addEventListener("scroll", onScroll, { passive: true });
    return () => window.removeEventListener("scroll", onScroll);
  }, []);

  useEffect(() => {
    const timer = setInterval(() => {
      setAnnouncementIdx((i) => (i + 1) % announcements.length);
    }, 5000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    document.body.style.overflow = mobileOpen ? "hidden" : "";
    return () => { document.body.style.overflow = ""; };
  }, [mobileOpen]);

  const announcement = announcements[announcementIdx];

  return (
    <>
      {/* Announcement bar */}
      <div className="announcement-bar" role="banner">
        <span>{announcement.text}</span>
        <Link href={announcement.link}>{announcement.linkText} →</Link>
      </div>

      {/* Main navigation */}
      <nav className={`nav-main ${pathname === "/" && !scrolled ? "nav-main--transparent" : ""} ${scrolled ? "nav-main--scrolled" : ""}`} role="navigation" aria-label="Main">
        <Link href="/" className="nav-logo" aria-label="Super6 Series LLC home">
          <Image
            src="/media/logos/logo-small-transparent.png"
            alt=""
            width={160}
            height={54}
            className="nav-logo-img"
            priority
          />
        </Link>

        <ul className="nav-links">
          {navLinks.map((link) => (
            <li key={link.href + link.label}>
              <Link
                href={link.href}
                className={pathname === link.href ? "active" : ""}
              >
                {link.label}
              </Link>
            </li>
          ))}
        </ul>

        <div className="nav-right">
          <button
            className="nav-hamburger"
            onClick={() => setMobileOpen(true)}
            aria-label="Open menu"
          >
            <Menu size={24} strokeWidth={2} />
          </button>
        </div>
      </nav>

      {/* Mobile menu */}
      <div className={`mobile-menu ${mobileOpen ? "mobile-menu--open" : ""}`} role="dialog" aria-modal="true">
        <div className="mobile-menu-header">
          <Link
            href="/"
            className="nav-logo"
            aria-label="Super6 Series LLC"
            onClick={() => setMobileOpen(false)}
          >
            <Image
              src="/media/logos/logo-small-transparent.png"
              alt="Super6 Series LLC"
              width={64}
              height={64}
              className="object-contain"
              style={{ width: 64, height: 64 }}
            />
          </Link>
          <button
            className="mobile-menu-close"
            onClick={() => setMobileOpen(false)}
            aria-label="Close menu"
          >
            <X size={24} strokeWidth={2} />
          </button>
        </div>
        <nav className="mobile-menu-nav">
          {navLinks.map((link) => (
            <Link key={link.href + link.label} href={link.href} onClick={() => setMobileOpen(false)}>
              {link.label}
            </Link>
          ))}
        </nav>
        <div className="mobile-menu-footer">
          <Link
            {...REGISTER_LINK_PROPS}
            className="mobile-menu-cta"
            onClick={() => setMobileOpen(false)}
          >
            Register Your Team
          </Link>
        </div>
      </div>
    </>
  );
}
