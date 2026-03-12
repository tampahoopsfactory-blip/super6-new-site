import type { Metadata } from "next";
import { Inter } from "next/font/google";
import Navigation from "@/components/layout/Navigation";
import Footer from "@/components/layout/Footer";
import "./globals.css";

/* ─── Inter — Nike uses Helvetica Now, Square uses Square Sans, both are
   geometric sans-serifs. Inter is the closest freely available match and
   provides the same clean, professional authority at every weight. ─── */
const inter = Inter({
  variable: "--font-inter",
  subsets: ["latin"],
  display: "swap",
});

export const metadata: Metadata = {
  metadataBase: new URL("https://www.thesuper6.com"),
  title: {
    default: "Super 6 | #1 Tournament Organization in Florida",
    template: "%s | Super 6",
  },
  description:
    "At Super 6, our mission is to bridge the education gap for underserved youth and prepare them for lasting success beyond the playing field through the power of sports.",
  keywords: [
    "youth basketball",
    "basketball tournament",
    "Super 6",
    "Florida basketball",
    "youth sports",
    "AAU basketball",
    "Orlando basketball",
    "Tampa basketball",
    "youth tournament",
    "college prep",
  ],
  openGraph: {
    type: "website",
    locale: "en_US",
    url: "https://www.thesuper6.com",
    siteName: "Super 6",
    title: "Super 6 | #1 Tournament Organization in Florida",
    description:
      "Bridging the education gap for underserved youth through the power of sports. Register your team today.",
    images: [
      {
        url: "/media/hero/hero-dunk.jpg",
        width: 5616,
        height: 3744,
        alt: "Super 6 Youth Basketball Tournament Action",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "Super 6 | Elite Youth Basketball",
    description:
      "#1 Tournament Organization in Florida. Bridging education and athletics.",
    images: ["/media/hero/hero-dunk.jpg"],
  },
  robots: {
    index: true,
    follow: true,
  },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className={inter.variable}>
      <body className="antialiased">
        <a href="#main-content" className="skip-to-content">
          Skip to content
        </a>
        <Navigation />
        <main id="main-content" role="main">
          {children}
        </main>
        <Footer />
      </body>
    </html>
  );
}
