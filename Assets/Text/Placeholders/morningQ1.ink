INCLUDE ../globals.ink

{ test_string == "": -> miseryChoice | -> already_chose }

=== miseryChoice ===
Pick a choice.
    + [Reaganomics]
        -> chosen("Reaganomics")
    + [The ten hells]
        -> chosen("The ten hells")
    + [Slug rain]
        -> chosen("Slug rain")
        
=== chosen(misery) ===
~ test_string = misery
You chose {misery}!
->END

=== already_chose ===
you already chose {test_string}!
-> END