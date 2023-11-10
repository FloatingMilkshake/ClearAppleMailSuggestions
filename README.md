# ClearAppleMailSuggestions
A simple program written in C# to clear contact suggestions in iCloud Mail and the stock Mail app on Apple devices.

## Usage
*todo: improve*

First, clone this repository. `cd` into the directory (into the second `ClearAppleMailSuggestions`, where you see the text files), then follow the steps below:
1. Go to icloud.com/mail and log in
2. Open your browser's developer tools and switch to the Network tab
3. Press Compose, and type anything into the `To:` field; your goal is to see suggestions
4. Press the red 'delete'/minus button next to any suggestion
5. In DevTools, find a `DELETE` request to `https://pXXX-mcc.icloud.com...`, where `XXX` are random numbers. For example, `https://p123-mcc.icloud.com...`.
6. Copy the value of the `Cookie` header, rename `cookies.example.txt` to `cookies.txt`, and replace the contents of the file with what you copied (note: cookies should all be on one line, separated by `; `)
7. Copy the URL from the request, rename `url.example.txt` to `url.txt`, and replace the contents of the file with what you copied
8. In DevTools again, find the `GET` request to `https://pXX-mcc.icloud.com...` whose response body contains a string `dsId` and a JSON list of `recipients`, and copy the entire value of the response
9. Rename `recipients.example.txt` to `recipients.txt` and replace the contents of the file with what you copied\*
10. Run the program, and you should see `OK: XX_XXXXX` in your console for each successful request

Once you've set things up, you'll need to use `dotnet` to run the program. `cd` into the directory, then use `dotnet build -c Release` and `dotnet run -c Release`.

> ## Note:
> A. This program is not affiliated with Apple in any way. Use at your own risk.
> 
> B. This program isn't guaranteed to work perfectly, as I threw it together quickly with the goal of only using it for myself. I'm also not the best at this kind of thing. If you run into issues you're welcome to open an [Issue](https://github.com/FloatingMilkshake/ClearMailSuggestions/issues) or contact me (see contact methods on my [website](https://floatingmilkshake.com)).
> 
> C. \*This program does not save any personal data, but feel free to redact emails, names, etc. The program only reads the `MR_XXX` or `GP_XXX` values to send them right back to Apple in `DELETE` requests. No data is stored persistently by the program. The same applies for cookies; the program only needs these to authenticate the requests to Apple so that they think you are the one sending them; cookies are not saved, ever. If you're worried, you can sign out of icloud.com afterwards and the cookies will be invalidated.

Big thanks to [@spotlightishere](https://github.com/spotlightishere) for the idea and some guidance!