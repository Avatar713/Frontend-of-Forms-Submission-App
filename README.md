# Frontend-of-Forms-Submission-App
The code for the frontend of the Forms Submission App given as a task.
I've incorporated the requested features and addressed potential issues in the following enhanced README file:

**Description:**

This desktop application provides a user-friendly interface for managing form submissions with features for:

- **Viewing Submissions:** Navigate through and access details of previously submitted forms.
- **Creating New Submissions:** Capture user details, including name, email, phone number, and GitHub repository link.
- **Tracking Elapsed Time:** Use a persistent stopwatch while creating submissions to measure time spent.
- **Submitting Forms:** Send captured data to the backend for processing.
- **Keyboard Shortcuts (Optional):** Enhance efficiency with keyboard shortcuts for key actions (e.g., Ctrl+S to submit on the Create New Submission form).
- **Form Management:**
    - **Deletion:** Delete submitted forms for record-keeping or data management purposes (consider implementing confirmation prompts and backup mechanisms).
    - **Editing:** Edit existing form entries to correct information or update details.
- **Form Styling:** Customize the appearance of both the View Submissions and Create New Submission forms to improve visual appeal and user experience (customization methods will depend on your chosen development environment).

**Getting Started:**

1. **Prerequisites:**
   - A compatible development environment with tools for creating desktop applications (e.g., Visual Studio, Python with PySimpleGUI, etc.).
   - Basic understanding of the chosen programming language and desktop application development concepts.

2. **Installation:**
   - Clone this repository using Git: `git clone https://github.com/your-username/form-manager-desktop`
   - Follow the specific instructions for your chosen development environment to set up the project and install any required dependencies.

3. **Running the Application:**
   - Refer to the documentation specific to your development environment for launching the application (e.g., building an executable in Visual Studio, running the main script in Python).

**Features:**

- **View Submissions Form:**
   - Displays details of the currently viewed submission.
   - Navigates through submissions using "Previous" and "Next" buttons (keyboard shortcuts may be implemented based on your environment).
   - The first submitted form entry is displayed by default.
- **Create New Submission Form:**
   - Provides editable fields for name, email, phone number, and GitHub repository link.
   - Includes a persistent stopwatch with a start/pause button (does not reset when paused).
   - Enables form submission to the backend using a "Submit" button (keyboard shortcut implementation optional).
- **Form Management:**
   - **Deletion:** Provides an option to delete submitted forms with appropriate confirmation prompts and potential backup mechanisms (implementation details depend on your chosen approach).
   - **Editing:** Allows users to modify existing form entries to correct information or update content.

**Customization:**

- The application's appearance and behavior can be customized within the code (e.g., form layout, styling, button labels).
- Keyboard shortcut implementation might require additional configuration depending on your development environment.
