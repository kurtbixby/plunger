const Physicality = Object.freeze(
    {
        0: "Unknown",
        Unknown: 0,
        1: "Physical",
        Physical: 1,
        2: "Digital",
        Digital: 2
    }
);

const PlayStates = Object.freeze(
    {
            0: "Unspecified",
            "Unspecified": 0,
            1: "Unplayed",
            "Unplayed": 1,
            2: "In Progress",
            "In Progress": 2,
            3: "Completed",
            "Completed": 3,
            4: "Paused",
            "Paused": 4,
    }
);

export default { Physicality, PlayStates };