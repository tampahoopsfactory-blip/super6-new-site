import type { Metadata } from "next";
import { Inter, Source_Serif_4 } from "next/font/google";
import Navigation from "@/components/layout/Navigation";
import Footer from "@/components/layout/Footer";
import "./globals.css";

const inter = Inter({
  variable: "--font-inter",
  subsets: ["latin"],
  display: "swap",
  weight: ["400", "500", "600"],
});

const sourceSerif = Source_Serif_4({
  variable: "--font-source-serif",
  subsets: ["latin"],
  display: "swap",
  weight: ["300", "400", "500", "600"],
  style: ["normal", "italic"],
});

export const metadata: Metadata = {
  metadataBase: new URL("https://www.thesuper6.com"),
  title: {
    default: "Super6 Series LLC | #1 Tournament Organization in Florida",
    template: "%s | Super6 Series LLC",
  },
  description:
    "At Super6 Series LLC, our mission is to bridge the education gap for underserved youth and prepare them for lasting success beyond the playing field through the power of sports.",
  keywords: [
    "youth basketball",
    "basketball tournament",
    "Super6 Series LLC",
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
    siteName: "Super6 Series LLC",
    title: "Super6 Series LLC | #1 Tournament Organization in Florida",
    description:
      "Bridging the education gap for underserved youth through the power of sports. Register your team today.",
    images: [
      {
        url: "/media/hero/hero-dunk.jpg",
        width: 5616,
        height: 3744,
        alt: "Super6 Series LLC Youth Basketball Tournament Action",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "Super6 Series LLC | Elite Youth Basketball",
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
    <html lang="en" className={`${inter.variable} ${sourceSerif.variable}`}>
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
