import { useState } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import HomePage from "./HomePage";
import CurrentUserContext from "./CurrentUserContext";
import TopNavigation from "./TopNavigation";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

function App() {
  const currentUser = useState({loggedIn: false, userName: null});
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <CurrentUserContext.Provider value={currentUser}>
          <TopNavigation />
          <Routes>
            <Route path="/" element={<HomePage />} />
            {/* <Route path="/collection" />
            <Route path="/account" />
            <Route path="/collection" />
            <Route path="/collection" /> */}
          </Routes>
        </CurrentUserContext.Provider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
