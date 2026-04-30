# Region Simulation

This is where we simulate what is going on each region, they are independant of each other, mostly

if they are physically close they will have similar weather etc, and kakapo can be moved between them

And if a region has predators they *can* try and migrate, might just send out explorations or something every now and then???
Or at least occasionally get lost etc

So on the region the following entities need simulation
 - Kakapo
 - Humans
 - Predators
 - Other wildlife
 - Trees
 - Plants
 - Climate? (Or is that going to be separate?)	

Trees/plants/predators probably arent individual entities simulating, but rather the population as a whole being semi modelled
The different entities will also tick at different rates.
Humans and Kakapo are probably the most interesting etc, so will tick as often as possible.
The other ones will tick once per day/some other longer time period.

## Kakapo

### Breeding season

Initially males are booming and females are looking for the boys.

Then matings occur, and are hopefully detected by the trackers, which may influence artificial insemination etc.

Then females start nesting, which triggers the rangers to set up nest monitoring.

Then eggs are laid and can be checked for fertility, and might be abducted/swapped around.
Danger now for eggs being smashed or left too long if females are searching for food and can't find it.

Then eventually they start hatching, again gotta ensure that they are kept warm enough and get enough food.
This is easier if there are multiple in the same nest as they keep each other warm.
Also the specifics of the nest can matter, good nests stay warmer, rangers can help with this.

Then the chicks start to leave the nest occasionally, this is dangerous but necessary.

Eventually they fledge, so mum and babies leave the nest permanently to learn to forage for food.

Then mum says good bye and they have to find their own territories.
This is a complicated time because all territories will have to shuffle around continuously until an equilibrium is found.
The territory that each bird can *control* will be influenced by many things
 - personality
 - strength/age
 - food availability
 - terrain type

### Off season

No one is nesting so they all just wander around.
Similar territory negotiations, when they stabilise rangers might have to move feed stations etc.
But generally this should settle down and only have slight chances of drastically changing if a bird dies or is transfered

Based on when the next breeding season is expected, the birds will start getting into condition etc, and at the annual health checks the mothers might get fancy backpacks.

## Humans

Will have a routine/schedule for what they should do.
Initially its just per person, but that might go unwieldy, so they assign roles and the roles have a schedule.
Will have different schedules in off season and breeding season.
Different funding levels will allow more staff, which will allow more intensive nest management etc.
Volunteers are cheap but not as good as proper rangers.

As the non-breeding years pass the rangers can detect when they expect the next breeding season to occur.

## Predators

Low populations are less likely to be detected, but also less likely to cause issues.
If they get populous enough they will be noticed somehow.
But they are also *very* hard to get rid of completely.
During breeding seasons they are more active/simulate faster.

## Other wildlife

Other sea birds/bugs?
Maybe only important in such that they compete for nesting locations or get into fights with nesting mothers.

## Trees

This is mostly just the mast trees, i.e. the ones that trigger breeding

## Plants

This is everything else that the Kakapo eat.

### Misc thoughts

When Kakapo are simulated and they move around, they may find a situation where they cannot all be distributed in such a way that they all have as much space as they need.

Different Kakapo might want more range, be more adventurous, be more fearful/fearless.

If they cannot get all the space they need, and they are fearful, and their neighbours are fearless, they may not have enough food.
Alternatively for the opposite they might bully other Kakapo.

Different terrain can influence how much territory a Kakapo wants, and what shape, maybe they have a distance they are prepared to cover, but if there are cliffs/rivers it reduces that?

Desired territory can be influenced based on food availability, rocky places aren't that desirable.