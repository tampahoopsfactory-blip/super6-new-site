import { NextRequest, NextResponse } from "next/server";

// Stripe checkout session creation
// In production, uncomment and configure with your Stripe secret key:
//
// import Stripe from "stripe";
// const stripe = new Stripe(process.env.STRIPE_SECRET_KEY!, {
//   apiVersion: "2024-12-18.acacia",
// });

export async function POST(req: NextRequest) {
  try {
    const body = await req.json();
    const { tier, teamName, coachName, email, division, location } = body;

    // Price mapping for Stripe price IDs
    // Replace these with your actual Stripe price IDs
    const priceMap: Record<string, { amount: number; name: string }> = {
      "single-tournament": {
        amount: 9900, // $99.00 in cents
        name: "Super6 Single Tournament Registration",
      },
      "season-pass": {
        amount: 89900, // $899.00 in cents — 10 events
        name: "Super6 Season Package Registration (10 events)",
      },
    };

    const selected = priceMap[tier];
    if (!selected) {
      return NextResponse.json({ error: "Invalid tier" }, { status: 400 });
    }

    // In production, create a Stripe Checkout session:
    //
    // const session = await stripe.checkout.sessions.create({
    //   payment_method_types: ["card"],
    //   line_items: [
    //     {
    //       price_data: {
    //         currency: "usd",
    //         product_data: {
    //           name: selected.name,
    //           description: `Team: ${teamName} | Coach: ${coachName} | Division: ${division} | Location: ${location}`,
    //         },
    //         unit_amount: selected.amount,
    //       },
    //       quantity: 1,
    //     },
    //   ],
    //   mode: "payment",
    //   success_url: `${process.env.NEXT_PUBLIC_URL}/register?success=true`,
    //   cancel_url: `${process.env.NEXT_PUBLIC_URL}/register?canceled=true`,
    //   customer_email: email,
    //   metadata: {
    //     teamName,
    //     coachName,
    //     division,
    //     location,
    //     tier,
    //   },
    // });
    //
    // return NextResponse.json({ url: session.url });

    // Placeholder response for development
    return NextResponse.json({
      message: "Checkout session would be created here",
      tier,
      amount: selected.amount,
      teamName,
      email,
    });
  } catch (error) {
    console.error("Checkout error:", error);
    return NextResponse.json(
      { error: "Failed to create checkout session" },
      { status: 500 }
    );
  }
}
