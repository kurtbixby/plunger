async function fetchNowPlaying({ queryKey }) {
  const placeholderData = {
    title: "Now Playing",
    entries: [
      {
        name: "Yakuza Kiwami",
        id: 12595,
        boxart:
          "https://images.igdb.com/igdb/image/upload/t_cover_big/co1pbc.png",
      },
      {
        name: "Persona 3 FES",
        id: 11437,
        boxart:
          "https://images.igdb.com/igdb/image/upload/t_cover_big/co6j8n.png",
      },
      {
        name: "The Great Ace Attorney Chronicles",
        id: 146075,
        boxart:
          "https://images.igdb.com/igdb/image/upload/t_cover_big/co2ylx.png",
      },
    ],
  };
  return Promise.resolve(placeholderData);

  const userId = queryKey[1];
  const response = await fetch();

  if (!response.ok) {
    throw new Error("Error retrieving homepage lists");
  }

  return response.json();
}

export default fetchNowPlaying;
