# Athlete Management Use Case Diagrams

## Overview Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        ATHLETE MANAGEMENT SYSTEM                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│    ┌─────────┐                                        ┌─────────┐          │
│    │         │                                        │         │          │
│    │ Athlete │                                        │  Coach  │          │
│    │         │                                        │         │          │
│    └────┬────┘                                        └────┬────┘          │
│         │                                                  │               │
│         │                                                  │               │
│    ┌────┴────────────────────────────────────────────────┬─┴───┐          │
│    │                                                     │     │          │
│    ▼                                                     ▼     ▼          │
│ ┌────────────────────────────────────────────────────────────────────┐    │
│ │                    PROFILE MANAGEMENT                              │    │
│ │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐             │    │
│ │  │UC1: Create   │  │UC2: Update   │  │UC3: Delete   │             │    │
│ │  │   Profile    │  │   Profile    │  │   Profile    │             │    │
│ │  └──────────────┘  └──────────────┘  └──────────────┘             │    │
│ └────────────────────────────────────────────────────────────────────┘    │
│                                                                           │
│ ┌────────────────────────────────────────────────────────────────────┐    │
│ │                    WEIGHT TRACKING                                 │    │
│ │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐             │    │
│ │  │UC4: Record   │  │UC5: View     │  │UC6: Calculate│             │    │
│ │  │   Weight     │  │   History    │  │    Change    │             │    │
│ │  └──────────────┘  └──────────────┘  └──────────────┘             │    │
│ └────────────────────────────────────────────────────────────────────┘    │
│                                                                           │
│ ┌────────────────────────────────────────────────────────────────────┐    │
│ │                    EXERCISE TRACKING                               │    │
│ │  ┌──────────────┐  ┌──────────────┐                               │    │
│ │  │UC7: Record   │  │UC8: View     │                               │    │
│ │  │   Exercise   │  │   History    │                               │    │
│ │  └──────────────┘  └──────────────┘                               │    │
│ └────────────────────────────────────────────────────────────────────┘    │
│                                                                           │
│ ┌────────────────────────────────────────────────────────────────────┐    │
│ │                    ATHLETE QUERIES (Coach)                         │    │
│ │  ┌──────────────┐  ┌──────────────┐                               │    │
│ │  │UC9: View     │  │UC10: View    │                               │    │
│ │  │    List      │  │   Details    │                               │    │
│ │  └──────────────┘  └──────────────┘                               │    │
│ └────────────────────────────────────────────────────────────────────┘    │
│                                                                           │
└───────────────────────────────────────────────────────────────────────────┘
```

---

## UC1: Create Athlete Profile

```
┌────────────────────────────────────────────────────────────────────────────┐
│                       UC1: Create Athlete Profile                           │
├────────────────────────────────────────────────────────────────────────────┤
│                                                                            │
│  ┌─────────┐     ┌──────────────┐     ┌──────────────┐                    │
│  │ Athlete │────▶│  Submit      │────▶│   Validate   │                    │
│  └─────────┘     │  Profile     │     │    Input     │                    │
│                  └──────────────┘     └──────┬───────┘                    │
│                                              │                             │
│                                   ┌──────────┴──────────┐                  │
│                                   │                     │                  │
│                              [Valid]               [Invalid]               │
│                                   │                     │                  │
│                                   ▼                     ▼                  │
│                         ┌──────────────┐       ┌──────────────┐           │
│                         │Create Athlete│       │ Return Error │           │
│                         │   Record     │       │     400      │           │
│                         └──────┬───────┘       └──────────────┘           │
│                                │                                          │
│                                ▼                                          │
│                      ┌──────────────────┐                                 │
│                      │  Publish Event   │                                 │
│                      │ AthleteCreated   │                                 │
│                      └────────┬─────────┘                                 │
│                               │                                           │
│                               ▼                                           │
│                      ┌──────────────────┐                                 │
│                      │  Return Athlete  │                                 │
│                      │      DTO         │                                 │
│                      └──────────────────┘                                 │
│                                                                           │
└───────────────────────────────────────────────────────────────────────────┘
```

### UC1 Specification

| Attribute | Value |
|-----------|-------|
| **Name** | Create Athlete Profile |
| **Primary Actor** | Athlete |
| **Secondary Actor** | System (Message Bus) |
| **Trigger** | Athlete submits profile creation form |
| **Preconditions** | User authenticated, no existing profile |
| **Postconditions** | Profile created, `AthleteCreatedMessage` published |

**Main Flow:**
1. Athlete provides name, username, optional image URL
2. System validates input (name required, username unique)
3. System creates Athlete aggregate with audit fields
4. System persists to database
5. System publishes `AthleteCreatedMessage` to message bus
6. System returns AthleteDto to client

**Business Rules:**
- Name: Required, max 256 characters
- Username: Required, unique within tenant
- TenantId: Automatically set from current user context

---

## UC4: Record Weight

```
┌────────────────────────────────────────────────────────────────────────────┐
│                          UC4: Record Weight                                 │
├────────────────────────────────────────────────────────────────────────────┤
│                                                                            │
│  ┌─────────┐     ┌──────────────┐     ┌──────────────┐                    │
│  │ Athlete │────▶│  Submit      │────▶│   Validate   │                    │
│  └─────────┘     │   Weight     │     │    Weight    │                    │
│                  └──────────────┘     └──────┬───────┘                    │
│                                              │                             │
│                                              ▼                             │
│                                    ┌──────────────────┐                    │
│                                    │ Weight > 0?      │                    │
│                                    └────────┬─────────┘                    │
│                                             │                              │
│                              ┌──────────────┴──────────────┐               │
│                              │                             │               │
│                          [Yes]                          [No]               │
│                              │                             │               │
│                              ▼                             ▼               │
│                    ┌──────────────────┐         ┌──────────────┐          │
│                    │Create AthleteWeight       │ Return Error  │          │
│                    │    Record        │         │     400      │          │
│                    └────────┬─────────┘         └──────────────┘          │
│                             │                                              │
│                             ▼                                              │
│                    ┌──────────────────┐                                    │
│                    │ Is Latest Date?  │                                    │
│                    └────────┬─────────┘                                    │
│                             │                                              │
│                    ┌────────┴────────┐                                     │
│                    │                 │                                     │
│                [Yes]             [No]                                      │
│                    │                 │                                     │
│                    ▼                 ▼                                     │
│          ┌──────────────┐    ┌──────────────┐                             │
│          │Update Current│    │  Keep        │                             │
│          │Weight + Date │    │  Existing    │                             │
│          └──────────────┘    └──────────────┘                             │
│                    │                 │                                     │
│                    └────────┬────────┘                                     │
│                             ▼                                              │
│                    ┌──────────────────┐                                    │
│                    │  Return Result   │                                    │
│                    └──────────────────┘                                    │
│                                                                            │
└────────────────────────────────────────────────────────────────────────────┘
```

### UC4 Specification

| Attribute | Value |
|-----------|-------|
| **Name** | Record Weight |
| **Primary Actor** | Athlete |
| **Trigger** | Athlete logs weight measurement |
| **Preconditions** | Athlete profile exists |
| **Postconditions** | Weight recorded, current weight potentially updated |

**Main Flow:**
1. Athlete provides weight (kg) and weighed date
2. System validates weight > 0
3. System creates AthleteWeight record
4. If weighed date >= LastWeighedOn:
   - Update CurrentWeight
   - Update LastWeighedOn
5. System returns weight record with change info

**Business Rules:**
- WeightInKgs must be > 0
- Historical weights can be recorded
- CurrentWeight only updates for same/later dates

---

## UC5: View Weight History

```
┌────────────────────────────────────────────────────────────────────────────┐
│                        UC5: View Weight History                             │
├────────────────────────────────────────────────────────────────────────────┤
│                                                                            │
│  ┌─────────┐     ┌──────────────┐     ┌──────────────┐                    │
│  │ Athlete │────▶│  Request     │────▶│   Validate   │                    │
│  └─────────┘     │   History    │     │   Athlete    │                    │
│                  └──────────────┘     └──────┬───────┘                    │
│                                              │                             │
│                                              ▼                             │
│                                    ┌──────────────────┐                    │
│                                    │ Query Weights    │                    │
│                                    │ Order By Date    │                    │
│                                    │ DESC             │                    │
│                                    └────────┬─────────┘                    │
│                                             │                              │
│                                             ▼                              │
│                                    ┌──────────────────┐                    │
│                                    │  Apply Limit     │                    │
│                                    │  (default: 10)   │                    │
│                                    └────────┬─────────┘                    │
│                                             │                              │
│                                             ▼                              │
│                                    ┌──────────────────┐                    │
│                                    │  Return Weight   │                    │
│                                    │     History      │                    │
│                                    └──────────────────┘                    │
│                                                                            │
└────────────────────────────────────────────────────────────────────────────┘
```

---

## UC6: Calculate Weight Change

```
┌────────────────────────────────────────────────────────────────────────────┐
│                       UC6: Calculate Weight Change                          │
├────────────────────────────────────────────────────────────────────────────┤
│                                                                            │
│  ┌─────────┐                                                               │
│  │ Athlete │                                                               │
│  └────┬────┘                                                               │
│       │                                                                    │
│       ▼                                                                    │
│  ┌──────────────┐     ┌──────────────┐     ┌──────────────┐              │
│  │  Request     │────▶│  Get Weight  │────▶│  Get Weight  │              │
│  │  Calculation │     │  at Start    │     │  at End      │              │
│  └──────────────┘     │  of Period   │     │  of Period   │              │
│                       └──────┬───────┘     └──────┬───────┘              │
│                              │                    │                       │
│                              └─────────┬──────────┘                       │
│                                        │                                  │
│                                        ▼                                  │
│                              ┌──────────────────┐                         │
│                              │  Both Weights    │                         │
│                              │  Available?      │                         │
│                              └────────┬─────────┘                         │
│                                       │                                   │
│                            ┌──────────┴──────────┐                        │
│                            │                     │                        │
│                        [Yes]                 [No]                         │
│                            │                     │                        │
│                            ▼                     ▼                        │
│                  ┌──────────────────┐  ┌──────────────────┐              │
│                  │ Calculate:       │  │ Return null      │              │
│                  │ Change = End -   │  │ (insufficient    │              │
│                  │         Start    │  │  data)           │              │
│                  │ Percentage =     │  └──────────────────┘              │
│                  │ (Change/Start)   │                                    │
│                  │ * 100            │                                    │
│                  └────────┬─────────┘                                    │
│                           │                                              │
│                           ▼                                              │
│                  ┌──────────────────┐                                    │
│                  │  Return Result   │                                    │
│                  │  - startWeight   │                                    │
│                  │  - endWeight     │                                    │
│                  │  - change        │                                    │
│                  │  - percentage    │                                    │
│                  └──────────────────┘                                    │
│                                                                          │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## UC7: Record Completed Exercise

```
┌────────────────────────────────────────────────────────────────────────────┐
│                     UC7: Record Completed Exercise                          │
├────────────────────────────────────────────────────────────────────────────┤
│                                                                            │
│  ┌─────────┐     ┌──────────────┐                                         │
│  │ Athlete │────▶│  Complete    │                                         │
│  └─────────┘     │  Exercise    │                                         │
│                  └──────┬───────┘                                         │
│                         │                                                 │
│                         ▼                                                 │
│               ┌──────────────────┐                                        │
│               │    Validate      │                                        │
│               │    Input         │                                        │
│               │  - Reps >= 0     │                                        │
│               │  - Sets >= 0     │                                        │
│               │  - Weight >= 0   │                                        │
│               └────────┬─────────┘                                        │
│                        │                                                  │
│         ┌──────────────┴──────────────┐                                   │
│         │                             │                                   │
│    [Valid]                       [Invalid]                                │
│         │                             │                                   │
│         ▼                             ▼                                   │
│   ┌──────────────────┐       ┌──────────────────┐                        │
│   │Create Completed  │       │   Return Error   │                        │
│   │Exercise Record   │       │       400        │                        │
│   │                  │       └──────────────────┘                        │
│   │Fields:           │                                                   │
│   │- ScheduledExId   │                                                   │
│   │- WeightInKgs     │                                                   │
│   │- Reps            │                                                   │
│   │- Sets            │                                                   │
│   │- Distance        │                                                   │
│   │- TimeInSeconds   │                                                   │
│   │- CompletionDate  │                                                   │
│   └────────┬─────────┘                                                   │
│            │                                                             │
│            ▼                                                             │
│   ┌──────────────────┐                                                   │
│   │    Check for     │                                                   │
│   │  Personal Record │                                                   │
│   └────────┬─────────┘                                                   │
│            │                                                             │
│            ▼                                                             │
│   ┌──────────────────┐                                                   │
│   │   Return Result  │                                                   │
│   └──────────────────┘                                                   │
│                                                                          │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## Weight Tracking Activity Diagram

```
┌──────────────────────────────────────────────────────────────────────────┐
│                    Weight Tracking Activity Flow                          │
├──────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│                           ┌─────────┐                                    │
│                           │  Start  │                                    │
│                           └────┬────┘                                    │
│                                │                                         │
│                                ▼                                         │
│                      ┌─────────────────┐                                 │
│                      │  Athlete Weighs │                                 │
│                      │    Themselves   │                                 │
│                      └────────┬────────┘                                 │
│                               │                                          │
│                               ▼                                          │
│                      ┌─────────────────┐                                 │
│                      │  Open App       │                                 │
│                      │  Weight Section │                                 │
│                      └────────┬────────┘                                 │
│                               │                                          │
│                               ▼                                          │
│                      ┌─────────────────┐                                 │
│                      │  Enter Weight   │                                 │
│                      │  in Kilograms   │                                 │
│                      └────────┬────────┘                                 │
│                               │                                          │
│                               ▼                                          │
│                      ◇─────────────────◇                                 │
│                     ╱  Select Date?    ╲                                 │
│                    ╱                    ╲                                │
│                   ◇──────────────────────◇                               │
│                   │                      │                               │
│               [Today]              [Other Date]                          │
│                   │                      │                               │
│                   ▼                      ▼                               │
│           ┌─────────────┐       ┌─────────────┐                         │
│           │  Use Today  │       │ Pick Date   │                         │
│           └──────┬──────┘       │ from Calendar│                        │
│                  │              └──────┬──────┘                         │
│                  │                     │                                │
│                  └──────────┬──────────┘                                │
│                             │                                           │
│                             ▼                                           │
│                    ┌─────────────────┐                                  │
│                    │  Submit Weight  │                                  │
│                    └────────┬────────┘                                  │
│                             │                                           │
│                             ▼                                           │
│                    ┌─────────────────┐                                  │
│                    │  System Records │                                  │
│                    │     Weight      │                                  │
│                    └────────┬────────┘                                  │
│                             │                                           │
│                             ▼                                           │
│                    ◇─────────────────◇                                  │
│                   ╱ Weight Changed?  ╲                                  │
│                  ╱   vs Last Week     ╲                                 │
│                 ◇──────────────────────◇                                │
│                 │            │          │                               │
│            [Gained]     [Same]    [Lost]                                │
│                 │            │          │                               │
│                 ▼            ▼          ▼                               │
│         ┌───────────┐ ┌───────────┐ ┌───────────┐                      │
│         │Show Trend │ │Show Stable│ │Show Trend │                      │
│         │    ↑      │ │    →      │ │    ↓      │                      │
│         └─────┬─────┘ └─────┬─────┘ └─────┬─────┘                      │
│               │             │             │                             │
│               └─────────────┼─────────────┘                             │
│                             │                                           │
│                             ▼                                           │
│                    ┌─────────────────┐                                  │
│                    │  Update Graph   │                                  │
│                    └────────┬────────┘                                  │
│                             │                                           │
│                             ▼                                           │
│                        ┌─────────┐                                      │
│                        │   End   │                                      │
│                        └─────────┘                                      │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## Athlete Data Model

```
┌────────────────────────────────────────────────────────────────────────┐
│                         ATHLETE AGGREGATE                               │
├────────────────────────────────────────────────────────────────────────┤
│                                                                        │
│  ┌────────────────────────────────────────────────┐                    │
│  │                  Athlete                        │                    │
│  │  (extends Profile)                             │                    │
│  ├────────────────────────────────────────────────┤                    │
│  │  + AthleteId: int                              │                    │
│  │  + Name: string                                │                    │
│  │  + Username: string                            │                    │
│  │  + ImageUrl: string?                           │                    │
│  │  + CurrentWeight: int?                         │                    │
│  │  + LastWeighedOn: DateTime?                    │                    │
│  │  + TenantId: int                               │                    │
│  │  + IsDeleted: bool                             │                    │
│  │  + CreatedOn: DateTime                         │                    │
│  │  + CreatedBy: string                           │                    │
│  │  + LastModifiedOn: DateTime                    │                    │
│  │  + LastModifiedBy: string                      │                    │
│  ├────────────────────────────────────────────────┤                    │
│  │  + Weights: ICollection<AthleteWeight>         │                    │
│  │  + CompletedExercises: ICollection<...>        │                    │
│  ├────────────────────────────────────────────────┤                    │
│  │  + Create(): Athlete                           │                    │
│  │  + RecordWeight(...)                           │                    │
│  │  + RecordCompletedExercise(...)                │                    │
│  │  + GetWeightHistory(count): List               │                    │
│  │  + CalculateWeightChange(days): int?           │                    │
│  │  + GetCompletedExercisesByDate(date): List     │                    │
│  └────────────────┬───────────────────────────────┘                    │
│                   │                                                    │
│          ┌────────┴────────┐                                           │
│          │                 │                                           │
│          ▼                 ▼                                           │
│  ┌───────────────┐  ┌───────────────────────┐                         │
│  │ AthleteWeight │  │  CompletedExercise    │                         │
│  ├───────────────┤  ├───────────────────────┤                         │
│  │+ Id           │  │+ Id                   │                         │
│  │+ AthleteId    │  │+ AthleteId            │                         │
│  │+ WeightInKgs  │  │+ ScheduledExerciseId  │                         │
│  │+ WeighedOn    │  │+ WeightInKgs          │                         │
│  │+ CreatedOn    │  │+ Reps                 │                         │
│  │+ CreatedBy    │  │+ Sets                 │                         │
│  │+ ...          │  │+ Distance             │                         │
│  └───────────────┘  │+ TimeInSeconds        │                         │
│                     │+ CompletionDateTime   │                         │
│                     │+ ...                  │                         │
│                     └───────────────────────┘                         │
│                                                                       │
└───────────────────────────────────────────────────────────────────────┘
```

---

## Permissions Matrix

| Use Case | Athlete | Coach | Admin |
|----------|:-------:|:-----:|:-----:|
| Create Own Profile | ✓ | ✓ | ✓ |
| Update Own Profile | ✓ | ✓ | ✓ |
| Delete Own Profile | - | - | ✓ |
| Record Own Weight | ✓ | ✓ | ✓ |
| View Own Weight History | ✓ | ✓ | ✓ |
| Calculate Own Change | ✓ | ✓ | ✓ |
| Record Own Exercise | ✓ | ✓ | ✓ |
| View Own Exercise History | ✓ | ✓ | ✓ |
| View All Athletes | - | ✓ | ✓ |
| View Any Athlete Details | - | ✓ | ✓ |
| Record Weight for Any | - | ✓ | ✓ |
| Record Exercise for Any | - | ✓ | ✓ |
| Delete Any Profile | - | - | ✓ |
