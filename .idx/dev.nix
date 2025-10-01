# To learn more about how to use Nix to configure your environment
# see: https://firebase.google.com/docs/studio/customize-workspace
{ pkgs, ... }: {
  # Which nixpkgs channel to use.
  channel = "unstable"; # Use unstable to get .NET 9

  # Use https://search.nixos.org/packages to find packages
  packages = [
    pkgs.dotnet-sdk_9
  ];

  # Sets environment variables in the workspace
  env = {};
  idx = {
    # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
    extensions = [
      "ms-dotnettools.csdevkit"
    ];

    # Enable previews
    previews = {
      enable = true;
      previews = {
        web = {
          command = ["dotnet" "watch" "--project" "src/Movura.Api/Movura.Api.csproj"];
          manager = "web";
          env = {
            ASPNETCORE_URLS = "http://0.0.0.0:$PORT";
          };
        };
      };
    };

    # Workspace lifecycle hooks
    workspace = {
      # Runs when a workspace is first created
      onCreate = {
        # Example: install JS dependencies from NPM
        # npm-install = "npm install";
      };
      # Runs when the workspace is (re)started
      onStart = {
        # Example: start a background task to watch and re-build backend code
        # watch-backend = "npm run watch-backend";
      };
    };
  };
}
