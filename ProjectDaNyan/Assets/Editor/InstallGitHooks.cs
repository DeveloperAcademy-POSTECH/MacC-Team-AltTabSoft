using System.Diagnostics;

public class InstallGitHooks {
    [UnityEditor.InitializeOnLoadMethod]
    private static void Install() {
        Process.Start("git", "config core.hooksPath .githooks/*");// 깃훅 규칙 활성화를 원할 경우 주석 해제
        // Process.Start("git", "git config --unset core.hooksPath"); // 깃훅 비활성화를 원할 경우 명령어 주석 해제
    }
}