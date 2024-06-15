async function fetchHomePageLists({ queryKey }) {
  const placeholderData = [
    {
      title: "Recently Started",
      id: 7,
      entries: [
        {
          name: "Yakuza Kiwami",
          id: 12595,
        },
        {
          name: "Persona 3 FES",
          id: 11437,
        },
      ],
    },
    {
      title: "Now Playing",
      id: 8,
      entries: [
        {
          name: "Yakuza Kiwami",
          id: 12595,
        },
        {
          name: "Persona 3 FES",
          id: 11437,
        },
        {
          name: "The Great Ace Attorney Chronicles",
          id: 146075,
        },
      ],
    },
  ];
  return Promise.resolve(placeholderData);

  const userId = queryKey[1];
  const response = await fetch();

  if (!response.ok) {
    throw new Error("Error retrieving homepage lists");
  }

  return response.json();
}

export default fetchHomePageLists;
