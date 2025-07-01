VAR has_talked_before = false

-> start

=== start ===
{has_talked_before:
    Oh, you’re back again! Settling in nicely, I hope?
    - else:
    Well now! A fresh face! Welcome to Elderwood Village, traveler. You must be tired from your journey.
}
* [Where exactly am I?]
    -> where
* [Can you point me to the inn?]
    -> inn
* [Who are you?]
    -> who

=== where ===
{has_talked_before:
    Still not sure where you are? Elderwood Village! Nestled right by the Mistfall Forest — safest place for miles.
    - else:
    You’re in Elderwood Village! Right by Mistfall Forest, safe and quiet — mostly.
}
* [Is it really safe?]
    -> safe
* [Sounds peaceful.]
    -> peaceful

=== inn ===
{has_talked_before:
    Still looking for the inn? Same as before — just follow the cobblestone path past the old oak, you can’t miss it.
    - else:
    The inn? Just follow the cobblestone path there, past the big oak tree. Old Marla will fix you up with a warm meal and a bed.
}
* [Thanks!]
    -> thanks
* [Is it expensive?]
    -> expensive

=== who ===
{has_talked_before:
    Still don’t remember me? I’m Cole, head of the village council — and your guide if you need one!
    - else:
    Who am I? The name’s Cole. I’ve lived here all my life — head of the village council, at your service.
}
* [Nice to meet you.]
    -> thanks
* [You don’t look that old!]
    -> joke

=== safe ===
{has_talked_before:
    Still asking about safety? We’re as safe as any place can be — keep your wits about you, that’s all.
    - else:
    Well, mostly. Just keep clear of the forest at night. The woods have... stories.
}
-> END

=== peaceful ===
{has_talked_before:
    Still dreaming of peace and quiet? You’ll find it here, if you know where to look.
    - else:
    Peaceful indeed — most days anyway! Folk here mind their own, and the air’s clean as you’ll find.
}
-> END

=== thanks ===
{has_talked_before:
    Always polite! You’ll fit right in here.
    - else:
    You’re welcome, traveler. Make yourself at home.
}
-> END

=== expensive ===
{has_talked_before:
    Still worried about coin? Marla’s fair — just don’t try to skip the bill.
    - else:
    Not too bad! Marla charges fair — a warm bed and stew won’t drain your purse.
}
-> END

=== joke ===
{has_talked_before:
    Ha! Still flattering me? I’ll take it.
    - else:
    Haha! Flatterer — I’ll take that as a compliment.
}
-> END
