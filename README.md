# GitLabGroupManagement

Example for configuration file:

Collection: BasicPeople
    ana.athena
    adolf.sidney
    borat.newyork
    maria.madrid
    TeamMars    

Collection: DevOpsPeople
    dragos.muscel
    daniel.berlin
    derek.liverpool
    celine.paris
    odette.london

Collection: TeamMars
    frederik.bologna
    isabella.chicago
    emily.tokio
    mio.bucharest
    odette.london

Collection: TeamVenus
    olivia.brasilia

Group: Dotnet
    BasicPeople: 30
    DevOpsPeople: 40
    odette.london: 50

Group: Java
    TeamMars: 40
    DevOpsPeople: 40
    BasicPeople: 20

Project: Python\Neptune
    emily.tokio: 50
    dragos.muscel: 20
    TeamVenus: 30
