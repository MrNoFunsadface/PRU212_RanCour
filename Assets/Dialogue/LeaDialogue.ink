VAR has_talked_before = false

-> start

=== start ===
{has_talked_before:
    Oh, it's you again! Still trying to figure out how to escape this place?
    - else:
    Huh? How did you make it ou - Doesnt matter, can you get me out too? This whole captive thing really slows down my progress!
}
* [Aren't you scared?]
    -> scared
* [Who are you?]
    -> who
* [Where am I?]
    -> where

=== scared ===
{has_talked_before:
    Still asking the same questions? As I told you before, why would I be scared? If they kill me, they would risk losing the cure to chicken pox!
    - else:
    Huh? Scared? Why would I be? If they kill me, they would risk losing the cure to chicken pox!
}
* [I don't think they know that.]
    -> doubtful
* [I'll see what I can do]
    -> help

=== who ===
{has_talked_before:
    We've been through this already! I'm the greatest doctor in all of Tamarail! The only one that could cure all your diseases! Do you have any new afflictions since we last spoke?
    - else:
    Who am I? I'm the greatest doctor in all of Tamarail! The only one that could cure all your diseases! Do you have any afflictions? Tell me about it.
}
* [I have never heard of you before]
    -> unheard
* [No afflictions yet]
    -> confirmation

=== where ===
{has_talked_before:
    You're asking about our location again? We're still in Alabastor prison! Have you forgotten already? Maybe you hit your head harder than I thought.
    - else:
    Huh? Oh you got the "knocked out and dragged in" treatment isn't it? We're in Alabastor prison! I used to wish I have a chance to visit this fascinating place, the forces over-seeing this place is something else entirely. Though I didn't think I would be dragged in here like this hehe.
}
* [Why would you want to visit a prison?]
    -> question
* [You're a... bit excited.]
    -> concerned

=== doubtful ===
{has_talked_before:
    Still doubting my reputation? Well, what are we talking about again?
    - else:
    What are we talking about again?
}
-> END

=== help ===
{has_talked_before:
    You're still willing to help? Excellent! This place remains such a bore.
    - else:
    You have my thanks! This place is such a bore
}
-> END

=== unheard ===
{has_talked_before:
    You're still claiming you haven't heard of me? You must really be from the most remote rural area, stranger.
    - else:
    You must be from a rural area then stranger, EVERYONE have heard of me surely.
}
-> END

=== confirmation ===
{has_talked_before:
    Still no new afflictions? That's good to hear! A healthy life is... what was that saying again?
    - else:
    That's unfortun- I mean, yes - a healthy wife is a healthy life... right? Was that how the saying goes? Eh.
}
-> END

=== question ===
{has_talked_before:
    As I mentioned before, prisons are fascinating, but this one is extra special! Those secrets I told you about are still tucked away behind these bars. Have you found any yet?
    - else:
    Prisons are interesting themselves, but this one is extra special, they said there are secrets tucked away behind the bars of these dungeon corridors - ooooooooh I can't wait!
}
-> END

=== concerned ===
{has_talked_before:
    You're still concerned about my excitement? What are we talking about again?
    - else:
    What are we talking about again?
}
-> END